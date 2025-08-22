using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.Helpers
{
    public class DatabaseHelper
    {
        static void Main()
        {
            CreateToDatabase();
        }

        public static void CreateToDatabase()
        {
            string cs = "server= DESKTOP-F9KSMDH\\SQLEXPRESS; database= WebSQLCRUD; trusted_connection= true; trustservercertificate= true";
            // Paths to your CSV files
            string authorsPath = "authors.csv";
            string booksPath = "books.csv";

            // Load authors
            var authors = new Dictionary<int, AuthorModel>();
            var authorLines = File.ReadAllLines(authorsPath);
            for (int i = 1; i < authorLines.Length; i++) // kip header
            {
                var values = authorLines[i].Split(',');
                var author = new AuthorModel
                {
                    id = int.Parse(values[0]),
                    name = values[1],
                    surname = values[2],
                    books = new List<BookModel>()
                };
                authors.Add(author.id, author);
            }

            // Load books
            var bookLines = File.ReadAllLines(booksPath);
            for (int i = 1; i < bookLines.Length; i++)
            {
                var values = bookLines[i].Split(',');
                int authorId = int.Parse(values[2]);

                var book = new BookModel
                {
                    id = int.Parse(values[0]),
                    title = values[1],
                    author = authors[authorId]
                };

                authors[authorId].books.Add(book);
            }

            // Convert to JSON 
            string json = JsonSerializer.Serialize(authors.Values, new JsonSerializerOptions { WriteIndented = true });

            // Send data to SQL Server
            SaveToDatabase(authors.Values, cs);
        }

        public static void SaveToDatabase(IEnumerable<AuthorModel> authors, string cs)
        {
            using var sqlConnection = new SqlConnection(cs);
            sqlConnection.Open();
            using var sqlTransaction = sqlConnection.BeginTransaction();

            // 1. Load current database state
            var dbAuthors = new Dictionary<int, AuthorModel>();
            using (var cmd = new SqlCommand("SELECT Id, Name, Surname FROM Authors", sqlConnection, sqlTransaction))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    dbAuthors.Add(reader.GetInt32(0), new AuthorModel
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        surname = reader.GetString(2),
                        books = new List<BookModel>()
                    });
                }
            }

            // Load books
            var dbBooks = new Dictionary<int, BookModel>();
            using (var cmd = new SqlCommand("SELECT Id, Title, AuthorId FROM Books", sqlConnection, sqlTransaction))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var book = new BookModel
                    {
                        id = reader.GetInt32(0),
                        title = reader.GetString(1),
                    };
                    int authorId = reader.GetInt32(2);

                    dbBooks.Add(book.id, book);
                    if (dbAuthors.ContainsKey(authorId))
                        dbAuthors[authorId].books.Add(book);
                }
            }

            // 2. Sync Authors
            foreach (var author in authors)
            {
                // Check if author already exists (by name+surname or by ID)
                var existingAuthor = dbAuthors.Values.FirstOrDefault(a => a.name == author.name && a.surname == author.surname);

                if (existingAuthor == null)
                {
                    // INSERT
                    using var insertCmd = new SqlCommand(@"
                INSERT INTO Authors (Name, Surname) 
                OUTPUT INSERTED.Id 
                VALUES (@Name, @Surname)", sqlConnection, sqlTransaction);

                    insertCmd.Parameters.AddWithValue("@Name", author.name);
                    insertCmd.Parameters.AddWithValue("@Surname", author.surname);
                    author.id = (int)insertCmd.ExecuteScalar();
                }
                else
                {
                    author.id = existingAuthor.id;
                    if (author.name != existingAuthor.name || author.surname != existingAuthor.surname)
                    {
                        // UPDATE
                        using var updateCmd = new SqlCommand(@"
                    UPDATE Authors SET Name=@Name, Surname=@Surname 
                    WHERE Id=@Id", sqlConnection, sqlTransaction);

                        updateCmd.Parameters.AddWithValue("@Id", existingAuthor.id);
                        updateCmd.Parameters.AddWithValue("@Name", author.name);
                        updateCmd.Parameters.AddWithValue("@Surname", author.surname);
                        updateCmd.ExecuteNonQuery();
                    }

                    // Mark this author as processed
                    dbAuthors.Remove(existingAuthor.id);
                }

                // 3. Sync Books for this author
                foreach (var book in author.books)
                {
                    var existingBook = dbBooks.Values.FirstOrDefault(b => b.title == book.title && author.id == dbAuthors.Values.FirstOrDefault()?.id);

                    if (existingBook == null)
                    {
                        // INSERT
                        using var insertCmd = new SqlCommand(@"
                    INSERT INTO Books (Title, AuthorId) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Title, @AuthorId)", sqlConnection, sqlTransaction);

                        insertCmd.Parameters.AddWithValue("@Title", book.title);
                        insertCmd.Parameters.AddWithValue("@AuthorId", author.id);
                        book.id = (int)insertCmd.ExecuteScalar();
                    }
                    else
                    {
                        book.id = existingBook.id;

                        if (book.title != existingBook.title)
                        {
                            // UPDATE
                            using var updateCmd = new SqlCommand(@"
                        UPDATE Books SET Title=@Title 
                        WHERE Id=@Id", sqlConnection, sqlTransaction);

                            updateCmd.Parameters.AddWithValue("@Id", existingBook.id);
                            updateCmd.Parameters.AddWithValue("@Title", book.title);
                            updateCmd.ExecuteNonQuery();
                        }

                        // Mark this book as processed
                        dbBooks.Remove(existingBook.id);
                    }
                }
            }

            // 4. Delete remaining unprocessed authors (and cascade books)
            foreach (var remainingAuthor in dbAuthors.Values)
            {
                using var deleteCmd = new SqlCommand("DELETE FROM Authors WHERE Id=@Id", sqlConnection, sqlTransaction);
                deleteCmd.Parameters.AddWithValue("@Id", remainingAuthor.id);
                deleteCmd.ExecuteNonQuery();
            }

            // 5. Delete remaining unprocessed books
            foreach (var remainingBook in dbBooks.Values)
            {
                using var deleteCmd = new SqlCommand("DELETE FROM Books WHERE Id=@Id", sqlConnection, sqlTransaction);
                deleteCmd.Parameters.AddWithValue("@Id", remainingBook.id);
                deleteCmd.ExecuteNonQuery();
            }

            sqlTransaction.Commit();
        }
    }
}

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

            // Convert to JSON (optional, for debugging or API)
            string json = JsonSerializer.Serialize(authors.Values, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);

            // Send data to SQL Server
            SaveToDatabase(authors.Values, cs);
        }

        internal static void SaveToDatabase(IEnumerable<AuthorModel> authors, string cs)
        {
            using var sqlConnection = new SqlConnection(cs);
            sqlConnection.Open();
            using var sqlTransaction = sqlConnection.BeginTransaction();

            foreach (var author in authors)
            {
                // Insert author, get generated Id
                using (var checkCmd = new SqlCommand(@"
                        SELECT Id FROM [dbo].[Authors] 
                        WHERE [Name] = @Name AND [Surname] = @Surname;", sqlConnection, sqlTransaction))
                {
                    checkCmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = author.name;
                    checkCmd.Parameters.Add("@Surname", SqlDbType.NVarChar, 100).Value = author.surname;

                    var existingId = checkCmd.ExecuteScalar();
                    if (existingId != null)
                    {
                        author.id = (int)existingId; // Use existing ID
                    }
                    else
                    {
                        using (var insertCmd = new SqlCommand(@"
                             INSERT INTO [dbo].[Authors] ([Name],[Surname])
                             OUTPUT INSERTED.[Id]
                             VALUES (@Name,@Surname);", sqlConnection, sqlTransaction))
                        {
                            insertCmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = author.name;
                            insertCmd.Parameters.Add("@Surname", SqlDbType.NVarChar, 100).Value = author.surname;
                            author.id = (int)insertCmd.ExecuteScalar();
                        }
                    }
                }

                // Insert books, get generated Ids (optional to read back)
                foreach (var book in author.books)
                {
                    using (var checkCmd = new SqlCommand(@"
                        SELECT Id FROM [dbo].[Books]
                        WHERE [Title] = @Title AND [AuthorId] = @AuthorId;", sqlConnection, sqlTransaction))
                    {
                        checkCmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = book.title;
                        checkCmd.Parameters.Add("@AuthorId", SqlDbType.Int).Value = author.id;

                        var existingId = checkCmd.ExecuteScalar();
                        if (existingId != null)
                        {
                            book.id = (int)existingId;
                        }
                        else
                        {
                            using (var insertCmd = new SqlCommand(@"
                                INSERT INTO [dbo].[Books] ([Title],[AuthorId])
                                OUTPUT INSERTED.[Id]
                                VALUES (@Title,@AuthorId);", sqlConnection, sqlTransaction))
                            {
                                insertCmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = book.title;
                                insertCmd.Parameters.Add("@AuthorId", SqlDbType.Int).Value = author.id;
                                book.id = (int)insertCmd.ExecuteScalar();
                            }
                        }
                    }
                }
            }

            sqlTransaction.Commit();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WebSQLCRUD.Helpers;
using WebSQLCRUD.Models;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly string _connectionString = "server= DESKTOP-F9KSMDH\\SQLEXPRESS; database= WebSQLCRUD; trusted_connection= true; trustservercertificate= true";

    [HttpPost]
    public IActionResult ImportData()
    {
        var authors = LoadAuthorsFromCsv("authors.csv");
        LoadBooksFromCsv("books.csv", authors);

        // Save to SQL Server
        DatabaseHelper.SaveToDatabase(authors.Values, _connectionString);

        // Return as JSON
        return Ok(authors.Values);
    }

    private Dictionary<int, AuthorModel> LoadAuthorsFromCsv(string path)
    {
        var authors = new Dictionary<int, AuthorModel>();
        var lines = System.IO.File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++) // Skip header
        {
            var values = lines[i].Split(',');
            authors.Add(int.Parse(values[0]), new AuthorModel
            {
                id = int.Parse(values[0]),
                name = values[1],
                surname = values[2],
                books = new List<BookModel>()
            });
        }
        return authors;
    }

    private void LoadBooksFromCsv(string path, Dictionary<int, AuthorModel> authors)
    {
        var lines = System.IO.File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            int authorId = int.Parse(values[2]);
            var book = new BookModel
            {
                id = int.Parse(values[0]),
                title = values[1],
                author = authors[authorId]
            };
            authors[authorId].books.Add(book);
        }
    }
}

using WebSQLCRUD.DTO.Connection;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.DTO.Book
{
    public class CreateBookDTO
    {
        public string title { get; set; }
        public AuthorConnDTO author { get; set; }
    }
}

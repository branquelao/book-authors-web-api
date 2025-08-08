using WebSQLCRUD.DTO.Connection;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.DTO.Book
{
    public class UpdateBookDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public AuthorConnDTO author { get; set; }
    }
}

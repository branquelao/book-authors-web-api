
namespace WebSQLCRUD.Models
{
    public class BookModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public AuthorModel author { get; set; }
    }
}

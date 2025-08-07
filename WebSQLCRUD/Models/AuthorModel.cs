using System.Text.Json.Serialization;

namespace WebSQLCRUD.Models
{
    public class AuthorModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }

        [JsonIgnore]
        public ICollection<BookModel> books { get; set; }
    }
}

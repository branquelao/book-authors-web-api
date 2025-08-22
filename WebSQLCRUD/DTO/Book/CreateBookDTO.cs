using System.ComponentModel.DataAnnotations;
using WebSQLCRUD.DTO.Connection;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.DTO.Book
{
    public class CreateBookDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string title { get; set; }

        [Required]
        public AuthorConnDTO author { get; set; }
    }
}

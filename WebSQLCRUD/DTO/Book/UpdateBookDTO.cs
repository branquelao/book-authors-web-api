using System.ComponentModel.DataAnnotations;
using WebSQLCRUD.DTO.Connection;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.DTO.Book
{
    public class UpdateBookDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string title { get; set; }

        [Required]
        public AuthorConnDTO author { get; set; }
    }
}

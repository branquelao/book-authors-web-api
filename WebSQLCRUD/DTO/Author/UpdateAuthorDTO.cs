using System.ComponentModel.DataAnnotations;

namespace WebSQLCRUD.DTO.Author
{
    public class UpdateAuthorDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string surname { get; set; }
    }
}

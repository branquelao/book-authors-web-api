using System.ComponentModel.DataAnnotations;

namespace WebSQLCRUD.DTO.Author
{
    public class CreateAuthorDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string surname { get; set; }
    }
}

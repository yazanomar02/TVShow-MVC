using System.ComponentModel.DataAnnotations;

namespace TV_Program_Management.Models
{
    public class UserModel
    {
        [Required]
        public required string FisrtName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string UserName { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public required string Password { get; set; }
    }
}

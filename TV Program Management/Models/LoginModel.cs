using System.ComponentModel.DataAnnotations;

namespace TV_Program_Management.Models
{
    public class LoginModel
    {
        [EmailAddress]
        public required string Email { get; set; }


        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}

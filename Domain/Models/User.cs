using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public Guid UserId { get; set; }

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

        public bool IsDeleted { get; set; } = false;

        public User()
        {
            UserId = Guid.NewGuid();
        }
    }
}

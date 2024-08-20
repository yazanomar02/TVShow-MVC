using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Language
    {
        public Guid LanguageId { get; set; }

        [Required]
        public required string Name { get; set; }

        public ICollection<TVShow> TVShows { get; set; } = new List<TVShow>();

        public bool IsDeleted { get; set; } = false;

        public Language()
        {
            LanguageId = Guid.NewGuid();
        }
    }
}

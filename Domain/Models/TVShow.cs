using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TVShow
    {
        public Guid TVShowId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required DateTime ReleassDate { get; set; }

        [Required]
        public required double Rating { get; set; }
        
        [Required]
        public required string URL { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Attachment? Attachment { get; set; }

        public ICollection<Language> Languages { get; set; } = new List<Language>();

        public TVShow()
        {
            TVShowId = Guid.NewGuid();
        }

    }
}

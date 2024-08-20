using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Path { get; set; }

        [Required]
        public required string FileType { get; set; }
        public bool IsDeleted { get; set; } = false;

        public TVShow TVShow { get; set; }

        public Guid TVShowId { get; set; }

        public Attachment()
        {
            AttachmentId = Guid.NewGuid();
        }
    }
}

using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace TV_Program_Management.Models
{
    public class TvShowModel
    {

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public required DateTime ReleassDate { get; set; }

        [Required]
        [Range(0, 10)]
        public required double Rating { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        [Url]
        public required string URL { get; set; }

        [Required]
        public required ICollection<string> Languages { get; set; } = new List<string>();

        public AttachmentModel? Attachment { get; set; }
    }
}

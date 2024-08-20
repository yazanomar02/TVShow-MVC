using System.ComponentModel.DataAnnotations;

namespace TV_Program_Management.Models
{
    public class AttachmentModel
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Path { get; set; }

        [Required]
        public required string FileType { get; set; }
    }
}

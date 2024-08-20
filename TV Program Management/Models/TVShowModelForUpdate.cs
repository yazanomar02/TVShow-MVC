using System.ComponentModel.DataAnnotations;

namespace TV_Program_Management.Models
{
    public class TVShowModelForUpdate
    {
        public Guid TVShowId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleassDate { get; set; }

        [Required]
        [Range(0, 10)]
        public double Rating { get; set; }

        [Required]
        [Url]
        public string URL { get; set; }

        [Required]
        public List<string> Languages { get; set; } = new List<string>();

        public IFormFile? Image { get; set; }
    }
}

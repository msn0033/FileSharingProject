using System.ComponentModel.DataAnnotations;

namespace FileSharingProject.Models
{
    public class InputFile
    {
        [Required]
        public IFormFile File { get; set; }
    }
}

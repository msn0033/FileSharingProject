using System.ComponentModel.DataAnnotations;

namespace FileSharingProject.Models
{
    public class AddPasswordViewModel
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}

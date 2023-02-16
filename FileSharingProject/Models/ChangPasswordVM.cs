using System.ComponentModel.DataAnnotations;

namespace FileSharingProject.Models
{
    public class ChangPasswordVM
    {
        [Required]
        public  string CurrentPassword { get; set; }
        [Required]
        public  string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public  string ConfirmPassword { get; set; }

    }
}

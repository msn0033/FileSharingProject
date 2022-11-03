using System.ComponentModel.DataAnnotations;

namespace FileSharingProject.Models
{
    public class RegisterVM
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}

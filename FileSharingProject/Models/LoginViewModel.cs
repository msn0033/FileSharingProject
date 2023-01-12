using FileSharingProject.Resources;
using System.ComponentModel.DataAnnotations;

namespace FileSharingProject.Models
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessageResourceName ="Email",ErrorMessageResourceType =typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name ="EmailLable",ResourceType = typeof(SharedResource))]
        public string Email { get; set; }


        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "PasswordLable", ResourceType = typeof(SharedResource))]
        public string Password { get; set; }
    }
}

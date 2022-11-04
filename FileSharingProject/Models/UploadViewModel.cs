using System;
using System.ComponentModel.DataAnnotations;

namespace FileSharingProject.Models
{
    public class UploadViewModel
    {


        [Required]
        public IFormFile File { get; set; }
    }
}


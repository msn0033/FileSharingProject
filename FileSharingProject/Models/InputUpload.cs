using System;
using System.ComponentModel.DataAnnotations;

namespace FileSharingProject.Models
{
    public class InputUpload
    {


        [Required]
        public IFormFile File { get; set; }
    }
}


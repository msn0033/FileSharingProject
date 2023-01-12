using System;
using System.ComponentModel.DataAnnotations;

namespace FileSharingProject.Models
{
    public class InputUpload
    {
        public string OrginalName { set; get; }
        public string FileName { set; get; }
        public string ContentType { set; get; }
        public long Size { set; get; }
        public string UserId { set; get; }

        
    }
}


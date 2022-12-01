using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharingProject.Data
{
    public class Uploads
    {
        public Uploads()
        {
            Id = Guid.NewGuid().ToString();
            UploadDate = DateTime.Now;
        }
        public string Id { get; set; }
        public string OrginalName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        [Column(TypeName ="decimal(18,4)")]
        public decimal Size { get; set; }
        public string UserId { get; set; }
        public DateTime UploadDate { get; set; }
        public IdentityUser User { get; set; }
        public long DownloadCount { get; set; }
        public string ImageName { get; set; }
    }
}

namespace FileSharingProject.Models
{
    public class UploadViewModel
    {
        public string UploadId { get; set; }
        public string OrginalName { get; set; }
        public string FileName { get; set; }
        public decimal SizeFile { get; set; }
        public string contentType { get; set; }
        public DateTime UploadDate { get; set; }
    }
}

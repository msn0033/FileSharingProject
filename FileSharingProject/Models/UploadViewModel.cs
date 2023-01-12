namespace FileSharingProject.Models
{
    public class UploadViewModel
    {
        public string UploadId { get; set; }
        public string OrginalName { get; set; }
        public string FileName { get; set; }
        public string contentType { get; set; }
        public decimal Size { get; set; }
        public DateTime UploadDate { get; set; }
        public long DownloadCount { get; internal set; }
    }
}

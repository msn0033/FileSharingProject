using FileSharingProject.Models;
using Microsoft.AspNetCore.Hosting;

namespace FileSharingProject.Helpers.UploadFile
{
    public static class UploadFile
    {


        private static  HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        private static  IWebHostEnvironment _webHostEnvironment => (IWebHostEnvironment)_httpContext.RequestServices.GetService(typeof(IWebHostEnvironment));

        public  static async Task<string>  UploadAnyFile( IFormFile file)
        {
            var Pathroot = _webHostEnvironment.WebRootPath;
            var NameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            var guid = Guid.NewGuid().ToString();
            var extension=Path.GetExtension(file.FileName);
            var Filename_Guid_Extension = NameWithoutExtension + guid + extension;

            var FullName = Path.Combine(Pathroot,"Uploads", Filename_Guid_Extension);
            using (var fs = File.Create(FullName))
            {
                await file.CopyToAsync(fs);
            }

            return Filename_Guid_Extension; 
        }
        public static InputUpload MapInformationFile(IFormFile file)
        {
            var inputUpload = new InputUpload {
                ContentType = file.ContentType,
                OrginalName = file.FileName,
                Size = file.Length
            };
            return inputUpload;
        }
    }
}

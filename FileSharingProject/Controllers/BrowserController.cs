using FileSharingProject.Data;
using FileSharingProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FileSharingProject.Controllers
{
    public class BrowserController : Controller
    {
        private readonly AppDbContext _DbContext;

        public BrowserController(AppDbContext appDbContext)
        {
            _DbContext = appDbContext;
        }
        public IActionResult Index()
        {
            var result = _DbContext.Uploads.OrderByDescending(x=>x.UploadDate)
                .Select(x => new UploadViewModel
                {
                    UploadId = x.Id,
                    OrginalName = x.OrginalName,
                    FileName = x.FileName,
                    contentType = x.ContentType,
                    SizeFile = x.Size,
                    UploadDate = x.UploadDate,
                    DownloadCount= x.DownloadCount,
                });
           
                return View(result);
           

        }
    }
}

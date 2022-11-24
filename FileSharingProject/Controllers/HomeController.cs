using FileSharingProject.Data;
using FileSharingProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileSharingProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger,AppDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            _dbContext.Uploads.OrderByDescending(x => x.DownloadCount).Take(5);
            return View();
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            var result=_dbContext.Uploads.Where(x=>x.OrginalName.Contains(search))
                .Select(x => new UploadViewModel
                {
                    UploadId = x.Id,
                    OrginalName = x.OrginalName,
                    FileName = x.FileName,
                    contentType = x.ContentType,
                    SizeFile = x.Size / 1000000,
                    UploadDate = x.UploadDate
                }).ToList();
            return View(result);
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using FileSharingProject.Data;
using FileSharingProject.Helpers.Mail;
using FileSharingProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace FileSharingProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly IMailHelper _mailHelper;

        //get userid
        private string? UserId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }

        public HomeController(ILogger<HomeController> logger,AppDbContext dbContext,IMailHelper mailHelper)
        {
            _logger = logger;
            this._dbContext = dbContext;
            this._mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            var result=_dbContext.Uploads.OrderByDescending(x => x.DownloadCount).Take(5)
                 .Select(x => new UploadViewModel
                 {
                     UploadId = x.Id,
                     OrginalName = x.OrginalName,
                     FileName = x.FileName,
                     contentType = x.ContentType,
                     SizeFile = x.Size,
                     UploadDate = x.UploadDate,
                     DownloadCount = x.DownloadCount,
                 });
            ViewBag.Popular = result;
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

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel  contact)
        {
          
            bool result=await _mailHelper.SendMail(new MailRequest
            {
                ToEmail=contact.Email,
                Body=contact.Message,
                Subject=contact.Subject
            });

            if(result == true)
            {
                await _dbContext.Contacts.AddAsync(new Data.Contact
                {
                    Name = contact.Name,
                    Subject = contact.Subject,
                    Email = contact.Email,
                    Message = contact.Message,
                    UserId = UserId

                });
                await _dbContext.SaveChangesAsync();
                TempData["Message"] = "Message is Success";
            }
            else
                TempData["Message"] = "Message Error";

            return RedirectToAction(nameof(Contact));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
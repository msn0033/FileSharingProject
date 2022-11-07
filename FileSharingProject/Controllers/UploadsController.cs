using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FileSharingProject.Data;
using FileSharingProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileSharingProject.Controllers
{
    [Authorize]
    public class UploadsController : Controller
    {
        private readonly AppDbContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public string UserId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }
        public UploadsController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._DbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {

            IQueryable<UploadViewModel> result = _DbContext.Uploads.Where(u => u.UserId == this.UserId)
                .Select(x => new UploadViewModel
                {
                    UploadId = x.Id,
                    OrginalName = x.OrginalName,
                    FileName = x.FileName,
                    contentType = x.ContentType,
                    SizeFile = x.Size / 1000000,
                    UploadDate = x.UploadDate
                });

            return View(result);
        }


        [HttpGet]
        // GET: /<controller>/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InputUpload upload)
        {
            if (ModelState.IsValid)
            {

                var guid = Guid.NewGuid().ToString();
                var name = upload.File.FileName.Substring(0, upload.File.FileName.IndexOf('.')) + '-' + guid;
                var Extension = Path.GetExtension(upload.File.FileName);

                var fileName = string.Concat(name, Extension);
                var root = _webHostEnvironment.WebRootPath;
                var fullPath = Path.Combine(root, "Uploads", fileName);
                using (var fs = System.IO.File.Create(fullPath))
                {
                    await upload.File.CopyToAsync(fs);
                }
                await _DbContext.Uploads.AddAsync(new Uploads
                {
                    OrginalName = upload.File.FileName,
                    FileName = fileName,
                    ContentType = upload.File.ContentType,
                    Size = upload.File.Length,
                    UserId = this.UserId

                });
                await _DbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(upload);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var selectedItem = await _DbContext.Uploads.Where(x => x.UserId == this.UserId)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (selectedItem is null)
                return NotFound();
            return View(selectedItem);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var selectedItem = await _DbContext.Uploads.Where(x => x.UserId == this.UserId)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (selectedItem is null)
                return NotFound();

            var root = _webHostEnvironment.WebRootPath;
            var fullPath = Path.Combine(root, "Uploads", selectedItem.FileName);
            System.IO.File.Delete(fullPath);
            _DbContext.Uploads.Remove(selectedItem);
            await _DbContext.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }
    }
}


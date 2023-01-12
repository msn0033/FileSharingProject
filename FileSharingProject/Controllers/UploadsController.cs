using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FileSharingProject.Data;
using FileSharingProject.Helpers.AutoMapper;
using FileSharingProject.Helpers.UploadFile;
using FileSharingProject.Models;
using FileSharingProject.ServicesManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileSharingProject.Controllers
{
    [Authorize]
    public class UploadsController : Controller
    {

        private readonly IUploadService _UploadService;
        private readonly IMapper _mapper;

        public string UserId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }

        public UploadsController(IUploadService uploadService, IMapper mapper)
        {
            _UploadService = uploadService;
            this._mapper = mapper;
        }
        //Get; Uploads
        public IActionResult Index()
        {
            var source = _UploadService.GetAllByUserId(UserId).ToList();

            var dest = _mapper.Map<List<UploadViewModel>>(source);

            return View(dest);
        }

        //Get:Uploads/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        //post: Uploads/Create
        [HttpPost]
        public async Task<IActionResult> Create(InputFile input)
        {
            if (ModelState.IsValid)
            {
                var inputUpload = UploadFile.MapInformationFile(input.File);
                inputUpload.FileName = await UploadFile.UploadAnyFile(input.File);
                inputUpload.UserId = UserId;
                var model = _mapper.Map<Upload>(inputUpload);
                await _UploadService.CreateAsync(model);
                await _UploadService.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(input);
        }

     

        [HttpGet]
        public async Task< ActionResult> DownloadFile(string uploadId)
        {
           var model= await _UploadService.GetByIdAsync(uploadId);
            if(model is null)
                return NotFound();
            _UploadService.IncurmentDownloadCount(model);
            await _UploadService.SaveAsync();
            var pathfull = "~/Uploads/" + model.FileName;
            return File(pathfull, model.ContentType, model.OrginalName);
        }
    }
}


using AutoMapper;
using AutoMapper.QueryableExtensions;
using FileSharingProject.Data;
using FileSharingProject.Helpers.AutoMapper;
using FileSharingProject.Models;
using FileSharingProject.ServicesManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace FileSharingProject.Controllers
{
    public class BrowserController : Controller
    {
      
        private readonly IUploadService _uploadService;
        private readonly IMapper _mapper;



        public BrowserController(IUploadService uploadService,IMapper mapper)
        {
        
            this._uploadService = uploadService;
            this._mapper = mapper;
         

        }
        public IActionResult Index(int PageCurrent = 1)
        {
            SkipTake( ref PageCurrent, out int PagesCount,  out int SkipItems, out int  TakeItems);

            ViewBag.PageCurrent = PageCurrent;
            var result =  _uploadService.GetAll().OrderByDescending(x=>x.UploadDate)
                //.Select(x => new UploadViewModel
                //{
                //    UploadId = x.Id,
                //    OrginalName = x.OrginalName,
                //    FileName = x.FileName,
                //    contentType = x.ContentType,
                //    Size = x.Size,
                //    UploadDate = x.UploadDate,
                //    DownloadCount= x.DownloadCount,
                //})
                .ProjectTo<UploadViewModel>(_mapper.ConfigurationProvider)
                .Skip(SkipItems).Take(TakeItems).ToList();
           
                return View(result); 
        }
        private void SkipTake( ref int PageCurrent, out int pagesCount, out int skipItems, out int takeItems )
        {
            if (PageCurrent <= 0)
                PageCurrent = 1;

            pagesCount = ((int)Math.Ceiling((decimal) _uploadService.GetUploadCount().GetAwaiter().GetResult() / PageCurrent));
            if (PageCurrent > pagesCount)
                PageCurrent = pagesCount;

            takeItems = 2;
          
            skipItems = (PageCurrent - 1) * takeItems;
            
        }
    }
}

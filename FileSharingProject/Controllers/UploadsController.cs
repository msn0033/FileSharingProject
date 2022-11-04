using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingProject.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileSharingProject.Controllers
{
    public class UploadsController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UploadViewModel upload)
        {

            return View();
        } 


    }
}


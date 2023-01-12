using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FileSharingProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCultureController : ControllerBase
    {
        private readonly IStringLocalizer<TestCultureController> _localization;
        public TestCultureController(IStringLocalizer<TestCultureController> localization)
        {
            _localization = localization;
        }
        [HttpGet]
        public IActionResult SetCulture()
        {
            var localize = _localization["Home"];
            return Ok(localize);
        }

      



    }
}

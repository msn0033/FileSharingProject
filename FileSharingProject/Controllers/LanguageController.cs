using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FileSharingProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IStringLocalizer<LanguageController> _localization;
        public LanguageController(IStringLocalizer<LanguageController> localization)
        {
            _localization = localization;
        }
        [HttpGet()]
        public IActionResult SetCulture()
        {
            return Ok(_localization["Home"].Value);
        }

        [HttpPost]
        public IActionResult SetCulture(string culture ,string url)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) }
                    );
            }
            return LocalRedirect(url);
        }



    }
}

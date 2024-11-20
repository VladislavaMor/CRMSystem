using Microsoft.AspNetCore.Mvc;

namespace CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPictureAsync(Guid Id)
        {

            byte[]? Image = await ImageDirectory.GetImageAsync(Id);

            if (Image == null) return NotFound();

            return File(Image, "image/png");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRM_Helper;
using CRM_API.Data;


namespace CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisualComponentsController : ControllerBase
    {

        // GET api/<VisualComponentsController>
        [HttpGet("Face")]
        public async Task<ActionResult<Face>> GetFace()
        {
            return Ok(await FaceFile.GetAsync());
        }

        // DELETE api/<VisualComponentsController>
        [HttpPut("Face")]
        [Authorize]
        public IActionResult PutFace(Face face)
        {
            FaceFile.Save(face);
            return NoContent();
        }
    }
}

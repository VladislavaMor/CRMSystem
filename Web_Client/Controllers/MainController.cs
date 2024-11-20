using Microsoft.AspNetCore.Mvc;
using CRM_Helper;
using CRM_APIRequests;

namespace Web_Client.Controllers
{
    public class MainController : Controller
    {
        private readonly SkillProfiWebClient _spClient = new();

        public async Task<IActionResult> Face()
        {
            Face face = await _spClient.Face.GetAsync();
            return View(face);
        }

        [HttpPost]
        public IActionResult GoWriteConsultation() 
        {
            return Redirect("/Consultation/Write");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using CRM_Helper;
using CRM_APIRequests;
using System.Security.Claims;

namespace Web_Client.Controllers
{
	public class ConsultationController : Controller
	{
		private readonly SkillProfiWebClient _spClient = new();

		public IActionResult Write()
		{
			return View(new AppealTransfer());
		}

		[HttpPost]
		public async Task<ActionResult> Write(AppealTransfer consult)
		{
			if (ModelState.IsValid)
			{ //checking model state

				await _spClient.Consultations.AddAsync(new() 
				{ 
					EMail = consult.EMail, 
					Name = consult.Name, 
					Description = consult.Description
				});

				return RedirectPermanent("~/");
			}

			return View(consult);
		}
	}
}

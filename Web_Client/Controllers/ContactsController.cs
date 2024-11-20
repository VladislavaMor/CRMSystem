using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRM_Helper;
using CRM_APIRequests;
using Web_Client.Models;

namespace Web_Client.Controllers
{
	public class ContactsController : Controller
	{
		private readonly SkillProfiWebClient _spClient = new();

		[HttpGet]
		public async Task<IActionResult> ContactsAsync()
		{
			Contact contacts = await _spClient.Contacts.GetAsync();


			ContactsModel cmodel = new()
			{
				Adress = contacts.Adress,
				Email = contacts.Email,
				LinkToMapContructor = contacts.LinkToMapContructor,
				PhoneNumber = contacts.PhoneNumber,

				SocialNetworks = contacts.SocialMedias.Select(sc => new ModelCustom<SocialMedia>()
				{
					Target = sc,
					ImageLink = _spClient.Pictures.GetURL(sc.ImageName)
				})
				.ToList()
			};

			return View(cmodel);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> EditInformationAsync()
		{
			Contact con = await _spClient.Contacts.GetAsync();

			ContactsTransfer ct = new()
			{
				Adress = con.Adress,
				Email = con.Email,
				PhoneNumber = con.PhoneNumber,
				LinkToMapContructor = con.LinkToMapContructor
			};

			return View(ct);
		}


		[HttpPost]
		[Authorize]
		public async Task<IActionResult> EditInformationAsync(ContactsTransfer model)
		{
			if (!ModelState.IsValid)
				return View(model);

			await _spClient.Contacts.EditAsync(model);

			return RedirectToAction("Contacts");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> SocialNetworksAsync()
		{
			List<SocialMedia> socialNetworks = await _spClient.SocialNetworks.GetListAsync();

			List<ModelCustom<SocialMedia>> blogs =
				socialNetworks.Select(b => new ModelCustom<SocialMedia>()
				{
					Id = b.Id.ToString(),
					Target = b,
					ImageLink = _spClient.Pictures.GetURL(b.ImageName)
				})
				.ToList();

			return View(blogs);

		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> EditSocialNetworkAsync(string? id)
		{
			if (id == null)
				return View(new ModelCustom<SocialMediaTransfer>() { Target = new() });

			List<SocialMedia> socList = await _spClient.SocialNetworks.GetListAsync();
			SocialMedia? soc = socList.FirstOrDefault(s => s.Id.ToString() == id);

			if (soc == null)
				return NotFound($"Такая социальная сеть не существует. Id = {id}");

			ModelCustom<SocialMediaTransfer> customSocialNetwork = new()
			{
				Id = id,
				Target = new()
				{
					Link = soc.Link,
				},
				ImageLink = _spClient.Pictures.GetURL(soc.ImageName)
			};

			return View(customSocialNetwork);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> EditSocialNetworkAsync(ModelCustom<SocialMediaTransfer> model, string? id, IFormFile? imageFile)
		 {
			if (!ModelState.IsValid)
				return View(model);

			if (id == null)
			{
				if (imageFile == null)
				{
					ModelState.AddModelError("ImageStatus", "Image is Required");
					return View(model);
				}
				await _spClient.SocialNetworks.AddAsync(model.Target, imageFile.OpenReadStream());
			}
			else
			{
				Stream? stream = null;
				if (imageFile != null)
					stream = imageFile.OpenReadStream();

				await _spClient.SocialNetworks.EditAsync(id, model.Target, stream);
			}

			return RedirectToAction("SocialNetworks");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> DeleteSocialNetworkAsync(string id)
		{
			await _spClient.SocialNetworks.DeleteByIdAsync(id);
			return RedirectToAction("SocialNetworks");
		}
	}
}

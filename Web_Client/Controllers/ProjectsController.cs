using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CRM_Helper;
using CRM_APIRequests;
using Web_Client.Models;

namespace Web_Client.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly SkillProfiWebClient _spClient = new();

        public async Task<IActionResult> ProjectsAsync()
        {
            List<Project> reqProjects = new(await _spClient.Projects.GetListAsync());
            List<ModelCustom<Project>> projects =
                reqProjects.Select(b => new ModelCustom<Project>()
                {
                    Target = b,
                    ImageLink = _spClient.Pictures.GetURL(b.ImageName)
                })
                .ToList();
            return View(projects);
        }
        public async Task<IActionResult> ProjectAsync(string id)
        {
            Project reqProject = await _spClient.Projects.GetByIdAsync(id);
			ModelCustom<Project> project =
                new()
                {
                    Target = reqProject,
                    ImageLink = _spClient.Pictures.GetURL(reqProject.ImageName)
                };

            return View(project);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new ModelCustom<ProjectTransfer>() { Target = new() });
            }

            Project reqProject = await _spClient.Projects.GetByIdAsync(id);

            ModelCustom<ProjectTransfer> mc = new()
            {
                Target = new()
                {
                    Title = reqProject.Title,
                    Description = reqProject.Description
                },
                Id = id,
                ImageLink = _spClient.Pictures.GetURL(reqProject.ImageName),

            };

            return View(mc);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAsync(ModelCustom<ProjectTransfer> model, string? id, IFormFile? imageFile)
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
                await _spClient.Projects.AddAsync(model.Target, imageFile.OpenReadStream());
            }
            else
            {
                Stream? stream = null;
                if (imageFile != null)  
                    stream = imageFile.OpenReadStream();
                
                await _spClient.Projects.EditAsync(id, model.Target, stream);
                return RedirectToAction("Project", new { id });
            }

            return RedirectToAction("Projects");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _spClient.Projects.DeleteByIdAsync(id);
            return RedirectToAction("Projects");
        }
    }
}

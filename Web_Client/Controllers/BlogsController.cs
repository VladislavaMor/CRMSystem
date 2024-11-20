using Microsoft.AspNetCore.Mvc;
using CRM_APIRequests;
using Web_Client.Models;
using CRM_Helper;
using Microsoft.AspNetCore.Authorization;

namespace Web_Client.Controllers
{
    public class BlogsController : Controller
    {
        private readonly SkillProfiWebClient _spClient = new();

        [HttpGet]
        public async Task<IActionResult> BlogsAsync()
        {
            List<Blog> reqblogs = new(await _spClient.Blogs.GetListAsync());
            List<ModelCustom<Blog>> blogs = 
                reqblogs.Select(b => new ModelCustom<Blog>()
                { 
                    Target = b,
                    ImageLink = _spClient.Pictures.GetURL(b.ImageName)
                })
                .ToList();

            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> BlogAsync(string id)
        {
            Blog reqblog = await _spClient.Blogs.GetByIdAsync(id.ToString());
			ModelCustom<Blog> blog = new()
            {
                Target = reqblog,
                ImageLink = _spClient.Pictures.GetURL(reqblog.ImageName)
            };

            return View(blog);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBlogAsync() 
        {
            BlogTransfer blog = new("доб", "адд");
            FileStream fs = System.IO.File.OpenRead("D:\\Users\\romik\\Pictures\\scale_1200.png");
            await _spClient.Blogs.AddAsync(blog,fs);
            return Redirect("Blogs");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditAsync(string? id)
        {
            if (id == null)
                return View(new ModelCustom<BlogTransfer>() {Target = new ()});
            

            Blog reqBlog = await _spClient.Blogs.GetByIdAsync(id);

            ModelCustom<BlogTransfer> bc = new()
            {
                Id = reqBlog.Id.ToString(),
                Target = new (reqBlog.Title, reqBlog.Description),
                ImageLink = _spClient.Pictures.GetURL(reqBlog.ImageName),
            };

            return View(bc);
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAsync(ModelCustom<BlogTransfer> model, string? id, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (id == null)
            {
                if (imageFile == null)
                {
                    ModelState.AddModelError("ImageStatus", "Image is Required");
                    return View(model);
                }
                await _spClient.Blogs.AddAsync(model.Target, imageFile.OpenReadStream());
            }
            else
            {
                Stream? stream = null;
                if (imageFile != null)
                    stream = imageFile.OpenReadStream();

                await _spClient.Blogs.EditAsync(id, model.Target, stream);
                return RedirectToAction("Blog", new { id });
            }

            return RedirectToAction("Blogs");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _spClient.Blogs.DeleteByIdAsync(id);
            return RedirectToAction("Blogs");
        }

    }
}

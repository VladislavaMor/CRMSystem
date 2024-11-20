using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM_Helper;
using CRM_API.Data;

namespace CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly WebApiDBContext _context;

        private readonly ILogger<ProjectsController> _logger;
        public BlogsController(WebApiDBContext context, ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            if (_context.Blogs == null)
                return Problem("Данные отсутствуют.");

            List<Blog> blogs = await _context.Blogs.ToListAsync();

            return Ok(blogs);
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog(Guid id)
        {
            if (_context.Blogs == null)
                return Problem("Данные отсутствуют.");

            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
                return NotFound();

            return Ok(blog);
        }

        // PUT: api/Blogs/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBlog(Guid id, ObjectWithImage<BlogTransfer> blogPic)
        {
            if (_context.Blogs == null)
                return Problem("Данные отсутствуют.");

            var blogTranfer = blogPic.Object;
            Blog? blog = await _context.Blogs.SingleOrDefaultAsync(blog1 => blog1.Id == id);

            if (blog == null || blogTranfer == null)
                return BadRequest();

            Blog updateBlog = new()
            {
                Id = blog.Id,
                Description = blogTranfer.Description,
                Title = blogTranfer.Title,
                ImageName = blog.ImageName,
                Created = blog.Created
            };

            _context.Entry(blog).CurrentValues.SetValues(updateBlog);

            try { await ImageDirectory.SaveImageAsync(blog, blogPic.Image); }
            catch (ImageNullException e)
            {
                _logger.LogWarning(exception: e, $"{nameof(blog)} saved without image");
                return BadRequest(e.Message);
            }

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                    return NotFound();

                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostBlog(ObjectWithImage<BlogTransfer> blogPic)
        {
            if (_context.Blogs == null)
                return Problem("Данные отсутствуют.");

            BlogTransfer blogBase = blogPic.Object;

            if (blogBase == null)
                return BadRequest();

            Blog blog = new()
            {
                Id = Guid.NewGuid(),
                Description = blogBase.Description,
                Title = blogBase.Title,
                Created = DateTime.UtcNow,
            };

            _context.Blogs.Add(blog);

            try { await ImageDirectory.SaveImageAsync(blog, blogPic.Image); }
            catch (ImageNullException e)
            {
                _logger.LogWarning(exception: e, $"{nameof(blog)} saved without image");
                return BadRequest(e.Message);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            if (_context.Blogs == null)
                return Problem("Данные отсутствуют.");

            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
                return NotFound();

            _context.Blogs.Remove(blog);

            try { blog.RemovePicture(); }
            catch (ImageNotFoundException e)
            {
                _logger.LogWarning(exception: e, "The image cannot be deleted because it is not found");
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(Guid id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

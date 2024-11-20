using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM_Helper;
using CRM_API.Data;

namespace CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly WebApiDBContext _context;

        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(WebApiDBContext context, ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            if (_context.Projects == null)
                return Problem("Entity set 'SkillProfiDbContext.Projects'  is null.");

            List<Project> projects = await _context.Projects.ToListAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {
            if (_context.Projects == null)
                return Problem("Entity set 'SkillProfiDbContext.Projects'  is null.");
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProject(Guid id, [FromBody] ObjectWithImage<ProjectTransfer> project)
        {
            if (_context.Projects == null)
                return Problem("Entity set 'SkillProfiDbContext.Projects'  is null.");

            var projBase = project.Object;
            var proj = await _context.Projects.SingleOrDefaultAsync(p => p.Id == id);

            if (proj == null)
                return BadRequest();

            Project updateProj = new()
            {
                Id = proj.Id,
                Title = projBase.Title,
                Description = projBase.Description,
                ImageName = proj.ImageName,
                Created = proj.Created,
            };

            _context.Entry(proj).CurrentValues.SetValues(updateProj);

            try { await ImageDirectory.SaveImageAsync(proj, project.Image); }
            catch (ImageNullException e)
            {
                _logger.LogWarning(exception: e, $"{nameof(project)}'s Image is not Excist and not pass from request");
                return BadRequest(e.Message);
            }

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                    return NotFound();

                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostProject(ObjectWithImage<ProjectTransfer> project)
        {
            if (_context.Projects == null)
                return Problem("Entity set 'SkillProfiDbContext.Projects'  is null.");

            ProjectTransfer projBase = project.Object;

            Project proj = new()
            {
                Id = Guid.NewGuid(),
                Title = projBase.Title,
                Description = projBase.Description,
                Created = DateTime.UtcNow,
            };

            _context.Projects.Add(proj);

            try { await ImageDirectory.SaveImageAsync(proj, project.Image); }
            catch (ImageNullException e)
            {
                _logger.LogWarning(exception: e, $"{nameof(proj)} saved without image");
                return BadRequest(e.Message);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            if (_context.Projects == null)
                return Problem("Entity set 'SkillProfiDbContext.Projects'  is null.");

            Project? project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);

            try { project.RemovePicture(); }
            catch (ImageNotFoundException e)
            {
                _logger.LogWarning(exception: e, "The image cannot be deleted because it's not found");
                return BadRequest(e.Message);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(Guid id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM_Helper;
using CRM_API.Data;

namespace CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationsController : ControllerBase
    {
        private readonly WebApiDBContext _context;

        public ConsultationsController(WebApiDBContext context)
        {
            _context = context;
        }

        // GET: api/Consultations
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Appeal>>> GetConsultations()
        {
            if (_context.Appeals == null)
                return Problem("Entity set 'SkillProfiDbContext.Consultations'  is null.");

            if (_context.Appeals == null)
            {
                return NotFound();
            }
            return Ok(await _context.Appeals.ToListAsync());
        }

        // GET: api/Consultations/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Appeal>> GetConsultation(Guid id)
        {
            if (_context.Appeals == null)
                return Problem("Entity set 'SkillProfiDbContext.Consultations'  is null.");

            if (_context.Appeals == null)
            {
                return NotFound();
            }
            var consultation = await _context.Appeals.FindAsync(id);

            if (consultation == null)
            {
                return NotFound();
            }

            return Ok(consultation);
        }

        // PUT: api/Consultations/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutConsultation(Guid id, Appeal appeal)
        {
            if (_context.Appeals == null)
                return Problem("Entity set 'SkillProfiDbContext.Consultations'  is null.");

            if (id != appeal.Id) return BadRequest();

            _context.Entry(appeal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultationExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/Appeals
        [HttpPost]
        public async Task<IActionResult> PostConsultation(AppealTransfer consultation)
        {
            if (_context.Appeals == null)
                return Problem("Entity set 'SkillProfiDbContext.Consultations'  is null.");

            var cons = new Appeal()
            {
                Id = Guid.NewGuid(),
                Name = consultation.Name,
                Description = consultation.Description,
                EMail = consultation.EMail,
                Status = AppealStatus.Received,
                Created = DateTime.Now
            };

            await _context.Appeals.AddAsync(cons);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Consultations/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteConsultation(Guid id)
        {
            if (_context.Appeals == null)
                return Problem("Entity set 'SkillProfiDbContext.Consultations'  is null.");

            if (_context.Appeals == null)
            {
                return NotFound();
            }
            var consultation = await _context.Appeals.FindAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }

            _context.Appeals.Remove(consultation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsultationExists(Guid id)
        {
            return (_context.Appeals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

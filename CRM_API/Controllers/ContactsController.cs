using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRM_Helper;
using CRM_API.Data;

namespace CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        public ContactsController(ILogger<ProjectsController> logger)
        {
            _logger = logger;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<Contact>> GetContacts()
        {
            Contact? contacts = await ContactsFile.GetContactsAsync();

            if (contacts == null) return NotFound();

            return Ok(contacts);
        }

        // PUT: api/Contacts
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutContacts(ContactsTransfer contacts)
        {
            if (contacts == null) return NotFound();
            await ContactsFile.EditMainContacts(contacts);
            return NoContent();
        }

        // GET: api/Contacts/SocialMedias
        [HttpGet("SocialMedias")]
        public async Task<ActionResult<IEnumerable<SocialMedia>>> GetSocialMedias()
        {
            var contacts = await ContactsFile.GetContactsAsync();
            if (contacts == null || contacts.SocialMedias == null) return NotFound();
            return Ok(contacts.SocialMedias);
        }

        // PUT: api/Contacts/SocialMedias
        [HttpPut("SocialMedias/{id}")]
        [Authorize]
        public async Task<IActionResult> PutSocialMedias(Guid id, ObjectWithImage<SocialMediaTransfer> SocialMedia)
        {
            SocialMediaTransfer socNetTransfer = SocialMedia.Object;
            SocialMedia? socNet = await ContactsFile.GetSocialMedia(id);

            if (socNet == null) return NotFound();
            socNet = new()
            {
                Id = socNet.Id,
                Link = socNetTransfer.Link,
                ImageName = socNet.ImageName
            };
            try { await ImageDirectory.SaveImageAsync(socNet, SocialMedia.Image); }
            catch (ImageNullException e)
            {
                _logger.LogWarning(exception: e, $"{nameof(SocialMedia)} saved without image");
                return BadRequest(e.Message);
            }
            await ContactsFile.EditSocialMedia(socNet);
            return NoContent();
        }

        // POST: api/Contacts/SocialMedias
        [HttpPost("SocialMedias")]
        [Authorize]
        public async Task<IActionResult> PostSocialMedias(ObjectWithImage<SocialMediaTransfer> SocialMedia)
        {
            var socNet = new SocialMedia()
            {
                Id = Guid.NewGuid(),
                Link = SocialMedia.Object.Link
            };

            await ContactsFile.AddSocialMedia(socNet, SocialMedia.Image);

            return NoContent();
        }

        // DELETE: api/Contacts/SocialMedias
        [HttpDelete("SocialMedias/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSocialMedias(Guid id)
        {
            if (!await ContactsFile.IsExcistSocialMediaById(id)) return NotFound(id);

            var SocialMedia = await ContactsFile.DeleteSocialMediaAsync(id);

            try { SocialMedia.RemovePicture(); }
            catch (ImageNotFoundException e)
            {
                _logger.LogWarning(exception: e, "The image cannot be deleted because it's not found");
            }

            return NoContent();
        }
    }
}

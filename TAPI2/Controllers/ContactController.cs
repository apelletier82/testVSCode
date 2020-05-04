using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TAPI2.Entities;
using System.Threading.Tasks;
using TAPI2.Models;
using TAPI2.Extensions;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using TAPI2.Services.Abstract;

namespace TAPI2.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private ILogger<ContactController> _logger;
        private IContactService _contactService;

        public ContactController(ILoggerFactory loggerFactory, IContactService contactService)            
        {
            _logger = loggerFactory.CreateLogger<ContactController>();
            _contactService = contactService;            
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  
        [Authorize(Policy="RequireContactViewerRole")]      
        public async Task<IEnumerable<ContactLightDto>> GetContactListAsync()
        {
            IEnumerable<Contact> contacts = await _contactService.ListAsync();
            return contacts.Select<Contact, ContactLightDto>(c => c.ToContactLightDto());
        }

        [HttpGet("{contactID}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy="RequireContactViewerRole")]
        public async Task<IActionResult> GetContact([FromRoute]long contactID)
        {                    
                                                                                     
            _logger.LogTrace("{0} get contact information", User.Identity.Name);
            Contact contact = await _contactService.GetByIDAsync(contactID);
            if (contact == null)
                return NotFound();

            return Ok(contact.ToContactDto());
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles="ContactOperator")]
        public async Task<IActionResult> AddContact([FromBody]ContactDto contactDto)
        {
            if (contactDto == null)
                return NoContent();

            Contact contact = await _contactService.AddAsync(contactDto.ToNewContact());
            if (contact == null)
            {
                _logger.LogError("New contact not created");
                _logger.LogDebug("ContactDto is {0}", Newtonsoft.Json.JsonConvert.SerializeObject(contactDto));
                return NotFound();
            }

            return CreatedAtAction(nameof(GetContact), contact.ToContactDto());
        }

        [HttpPut("{contactID}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles="ContactOperator")]
        public async Task<IActionResult> UpdateContact([FromRoute]long contactID, [FromBody]ContactDto contactDto)
        {   
            if (contactDto == null)
            {
                _logger.LogError("Cannot update contact ID [{0}]: contactDto is null", contactID);
                return NoContent();
            }
            else if (contactID != contactDto.ID)
            {
                _logger.LogError("Cannot update contact : ID [{0}] differs from contactDto.ID [{1}]", new[]{contactID, contactDto.ID});
                return BadRequest();                
            }
                
            Contact contact = await _contactService.GetByIDAsync(contactID);
            if (contactID == 0 || contact == null)
            {
                _logger.LogError("Cannot update contact : ID [{0}] not found", contact.ID);
                return NotFound();                
            }
            else if (!contact.RowVersion.SequenceEqual(contactDto.RowVersion))
            {
                _logger.LogWarning("Contact ID [{0}] already updated by another user", contactID);
                return NotFound();
            }

            contact = await _contactService.UpdateAsync(contactDto.UpdateContact(contact));
            return Ok(contact.ToContactDto());
        }

        [HttpDelete("{contactID}")]        
        [ProducesResponseType(StatusCodes.Status200OK)]              
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  
        [Authorize(Roles="ContactOperator")]     
        public async Task<IActionResult> DeleteContact([FromRoute]long contactID)
        {
            Contact contact = await _contactService.GetByIDAsync(contactID);
            if (contact == null)
            {
                _logger.LogError("Cannot delete contact: ID [{0}] not found", contactID);
                return NotFound();
            }
            
            await _contactService.DeleteAsync(contact);            
            return Ok();
        }
    }
}
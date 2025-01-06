using garagebackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garagebackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly GarageDbContext _context;

        public ContactsController(GarageDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contacts>>> GetContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contacts>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();
            return contact;
        }

        [HttpPost]
        public async Task<ActionResult<Contacts>> CreateContact(Contacts contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContact), new { id = contact.contactID }, contact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, Contacts contact)
        {
            if (id != contact.contactID) return BadRequest();

            _context.Entry(contact).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();

            try
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Issue deleting contact...it doesnt exist!");
            }
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.contactID == id);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCShopBlazor.Data;
using PCShopBlazor.Services;

namespace PCShopBlazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = RolesService.Admin + "," + RolesService.BasicUser)]
    [ApiController]
    public class PartsController : ControllerBase
    {
        private readonly PCShopBlazorContext _context;

        public PartsController(PCShopBlazorContext context)
        {
            _context = context;
        }

        // GET: api/Parts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Part>>> GetPart()
        {
            if (await CheckAdmin())
            {
                return await _context.Parts.ToListAsync();
            }
            else //if(await CheckAuthorization())
            {
                User user = await _context.Users.FirstOrDefaultAsync(e => e.Name == User.Identity.Name);
                return await _context.Parts.Where(e => e.BuyerId == user.Id).ToListAsync();
            }
        }

        // GET: api/Parts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Part>> GetPart(int id)
        {
            var part = await _context.Parts.FindAsync(id);

            if (!await CheckAuthorization(part))
            {
                return Unauthorized();
            }

            if (part == null)
            {
                return NotFound();
            }

            return part;
        }

        // PUT: api/Parts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPart(int id, Part part)
        {
            if (!await CheckAdmin())
            {
                return Unauthorized();
            }

            if (id != part.Id)
            {
                return BadRequest();
            }

            _context.Entry(part).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Parts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Part>> PostPart(Part part)
        {
            if (!await CheckAdmin())
            {
                return Unauthorized();
            }

            _context.Parts.Add(part);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPart", new { id = part.Id }, part);
        }

        // DELETE: api/Parts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Part>> DeletePart(int id)
        {
            if (!await CheckAdmin())
            {
                return Unauthorized();
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();

            return part;
        }

        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.Id == id);
        }

        public async Task<bool> CheckAuthorization(Part part)
        {
            User requestedUser = await _context.Users.FirstOrDefaultAsync(e => e.Name == User.Identity.Name);
            if (requestedUser.Type == "Admin" || requestedUser.Id == part.BuyerId)
                return true;
            else
                return false;
        }

        public async Task<bool> CheckAdmin()
        {
            User requestedUser = await _context.Users.FirstOrDefaultAsync(e => e.Name == User.Identity.Name);
            if (requestedUser.Type == "Admin")
                return true;
            else
                return false;
        }
    }
}

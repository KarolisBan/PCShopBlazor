using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCShopBlazor.Data;
using PCShopBlazor.Services;

namespace PCShopBlazor.Controllers
{       //
    [Route("api/[controller]")]
    [Authorize(Roles = RolesService.Admin + "," + RolesService.BasicUser)]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        private readonly PCShopBlazorContext _context;

        public ComputersController(PCShopBlazorContext context)
        {
            _context = context;
        }

        // GET: api/Computers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Computer>>> GetComputer()
        {
            return await _context.Computers.ToListAsync();
        }

        // GET: api/Computers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> GetComputer(int id)
        {
            var computer = await _context.Computers.FindAsync(id);

            if (computer == null)
            {
                return NotFound();
            }

            return computer;
        }

        // PUT: api/Computers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComputer(int id, Computer computer)
        {
            if (id != computer.Id)
            {
                return BadRequest();
            }

            _context.Entry(computer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComputerExists(id))
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

        // POST: api/Computers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Computer>> PostComputer(Computer computer)
        {
            if (computer.OrderId == 0)
            {
                return BadRequest("Order has not been specified.");
            }

            if (await _context.Orders.FirstOrDefaultAsync(e => e.Id == computer.OrderId) == null)
            {
                return NotFound("Order does not exist.");
            }

            try
            {
                _context.Computers.Add(computer);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Created($"/api/Computers/", new { id = computer.Id });
        }

        // DELETE: api/Computers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Computer>> DeleteComputer(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }

            try
            {
                _context.Computers.Remove(computer);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return computer;
        }

        private bool ComputerExists(int id)
        {
            return _context.Computers.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/api/Computers/{computerId:int}/Parts")]
        public async Task<ActionResult<IEnumerable<Part>>> GetAllPartsByComputer(int computerId)
        {
            return await _context.Parts.Where(e => e.ComputerId == computerId).ToListAsync();
        }

        [HttpGet]
        [Route("/api/Computers/{computerId:int}/Parts/{partId:int}")]
        public async Task<ActionResult<Part>> GetPartByComputer(int computerId, int partId)
        {
            return await _context.Parts.FirstOrDefaultAsync(e => e.ComputerId == computerId && e.Id == partId);
        }

        [HttpPost]
        [Route("/api/Computers/{computerId:int}/Parts")]
        public async Task<ActionResult<Part>> PostPartForComputer(int computerId, Part part)
        {
            if (computerId == 0 && part.ComputerId == 0)
                return BadRequest();

            part.ComputerId = computerId;

            Computer foundComputer = await _context.Computers.FirstOrDefaultAsync(e => e.Id == computerId);

            if (foundComputer is null)
                return NotFound("Computer does not exist.");

            part.ComputerId = computerId;

            try
            {
                _context.Parts.Add(part);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Created($"/api/Computers/{computerId}/Parts", new { Id = part.Id });
        }
    }
}

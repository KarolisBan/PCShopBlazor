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
    public class OrdersController : ControllerBase
    {
        private readonly PCShopBlazorContext _context;

        public OrdersController(PCShopBlazorContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            if (await CheckAdmin())
            {
                return await _context.Orders.ToListAsync();
            }
            else //if(await CheckAuthorization())
            {
                User user = await _context.Users.FirstOrDefaultAsync(e => e.Name == User.Identity.Name);
                return await _context.Orders.Where(e => e.BuyerId == user.Id).ToListAsync();
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (!await CheckAuthorization(order))
            {
                return Unauthorized();
            }

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (!await CheckAdmin())
            {
                return Unauthorized();
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (!await CheckAdmin())
            {
                return Unauthorized();
            }
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }


            return Created($"/api/Orders", new { id = order.Id });
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            if (!await CheckAdmin())
            {
                return Unauthorized();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        public async Task<bool> CheckAuthorization(Order order)
        {
            User requestedUser = await _context.Users.FirstOrDefaultAsync(e => e.Name == User.Identity.Name);
            if (requestedUser.Type == "Admin" || requestedUser.Id == order.BuyerId)
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

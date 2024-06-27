using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SalesWebApiTutorial.Data;
using SalesWebApiTutorial.Models;

namespace SalesWebApiTutorial.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController :ControllerBase {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Demonstrates how to call a method in a different controller
        /// </summary>
        /// <param name="orderId">The primary key of Order</param>
        /// <returns>No content</returns>
        [HttpPut("recalc/{orderId}")]
        public async Task<IActionResult> ReturnControllerClassname(int orderId) {
            var orderlineController = new OrderlinesController(_context);
            return await orderlineController.RecalculateOrderTotal(orderId);
        }

        // GET: api/orders/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByStatus(string status) {
            return await _context.Orders.Where(x => x.Status == status).ToListAsync();
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id) {
            var order = await _context.Orders.FindAsync(id);

            if(order == null) {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/Shipped/5
        [HttpPut("shipped/{id}")]
        public async Task<IActionResult> ShippedOrder(int id, Order order) {
            order.Status = "SHIPPED";
            return await PutOrder(id, order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order) {
            if(id != order.Id) {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!OrderExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order) {

            order.Status = "NEW";
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id) {
            var order = await _context.Orders.FindAsync(id);
            if(order == null) {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id) {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}

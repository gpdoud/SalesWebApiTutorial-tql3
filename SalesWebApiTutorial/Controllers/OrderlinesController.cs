﻿using System;
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
    public class OrderlinesController :ControllerBase {
        private readonly AppDbContext _context;

        public OrderlinesController(AppDbContext context) {
            _context = context;
        }

        private async Task<IActionResult> RecalculateOrderTotal(int orderId) {
            // read the order to be recalculated
            var order = await _context.Orders.FindAsync(orderId);
            // if the order is not found, return NOT FOUND
            if(order is null) {
                return NotFound();
            }
            // if we get here, we did find the order
            // now calculate the total and store it in order.Total
            order.Total = (from ol in _context.Orderlines
                           join i in _context.Items
                             on ol.ItemId equals i.Id
                           where ol.OrderId == orderId
                           select new {
                               LineTotal = ol.Quantity * i.Price
                           }).Sum(x => x.LineTotal);
            // now update the order with the new Total
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: api/Orderlines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderline>>> GetOrderlines() {
            return await _context.Orderlines.ToListAsync();
        }

        // GET: api/Orderlines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orderline>> GetOrderline(int id) {
            var orderline = await _context.Orderlines.FindAsync(id);

            if(orderline == null) {
                return NotFound();
            }

            return orderline;
        }

        // PUT: api/Orderlines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderline(int id, Orderline orderline) {
            if(id != orderline.Id) {
                return BadRequest();
            }

            _context.Entry(orderline).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
                await RecalculateOrderTotal(orderline.OrderId);
            } catch(DbUpdateConcurrencyException) {
                if(!OrderlineExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orderlines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Orderline>> PostOrderline(Orderline orderline) {
            _context.Orderlines.Add(orderline);
            await _context.SaveChangesAsync();
            await RecalculateOrderTotal(orderline.OrderId);

            return CreatedAtAction("GetOrderline", new { id = orderline.Id }, orderline);
        }

        // DELETE: api/Orderlines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderline(int id) {
            var orderline = await _context.Orderlines.FindAsync(id);
            if(orderline == null) {
                return NotFound();
            }

            _context.Orderlines.Remove(orderline);
            await _context.SaveChangesAsync();
            await RecalculateOrderTotal(orderline.OrderId);

            return NoContent();
        }

        private bool OrderlineExists(int id) {
            return _context.Orderlines.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeBeanAPI.Data;
using CoffeeBeanAPI.Models;

namespace CoffeeBeanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeansController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BeansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Beans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bean>>> GetBeans()
        {
            return await _context.Beans.ToListAsync();
        }

        // GET: api/Beans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bean>> GetBean(Guid id)
        {
            var bean = await _context.Beans.FindAsync(id);

            if (bean == null)
            {
                return NotFound();
            }

            return bean;
        }

        // PUT: api/Beans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBean(Guid id, Bean bean)
        {
            if (id != bean.Id)
            {
                return BadRequest();
            }

            _context.Entry(bean).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeanExists(id))
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

        // POST: api/Beans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bean>> PostBean(Bean bean)
        {
            _context.Beans.Add(bean);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBean", new { id = bean.Id }, bean);
        }

        // DELETE: api/Beans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBean(Guid id)
        {
            var bean = await _context.Beans.FindAsync(id);
            if (bean == null)
            {
                return NotFound();
            }

            _context.Beans.Remove(bean);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // SEARCH BEANS: Filter by name, country, or color
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Bean>>> SearchBeans(
            [FromQuery] string? name,
            [FromQuery] string? country,
            [FromQuery] string? colour,
            [FromQuery] bool? isBOTD,
            [FromQuery] decimal? minCost,
            [FromQuery] decimal? maxCost)
        {
            var query = _context.Beans.AsQueryable();

            // Apply non-cost filters first
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(b => b.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                query = query.Where(b => b.Country.Contains(country));
            }

            if (!string.IsNullOrWhiteSpace(colour))
            {
                query = query.Where(b => b.colour.Contains(colour));
            }

            if (isBOTD.HasValue)
            {
                query = query.Where(b => b.isBOTD == isBOTD.Value);
            }

            // Get results and then filter by cost
            var results = await query.ToListAsync();

            if (minCost.HasValue || maxCost.HasValue)
            {
                results = results.Where(b => {
                    var cost = decimal.Parse(b.Cost.Replace("£", ""));
                    return (!minCost.HasValue || cost >= minCost.Value) &&
                           (!maxCost.HasValue || cost <= maxCost.Value);
                }).ToList();
            }

            if (!results.Any())
            {
                return NotFound("No matching beans found.");
            }

            return Ok(results);
        }

        private bool BeanExists(Guid id)
        {
            return _context.Beans.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yachtberles.Data;
using Yachtberles.Models;

namespace Yachtberles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BerlesekController : ControllerBase
    {
        private readonly YachtberlesContext _context;

        public BerlesekController(YachtberlesContext context)
        {
            _context = context;
        }

        // GET: api/Berlesek
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Berles>>> GetBerles()
        {
            return await _context.Berles.ToListAsync();
        }

        // GET: api/Berlesek/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Berles>> GetBerles(int id)
        {
            var berles = await _context.Berles.FindAsync(id);

            if (berles == null)
            {
                return NotFound();
            }

            return berles;
        }


        // POST: api/Berlesek
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Berles>> PostBerles(Berles berles)
        {
            // A bérlés kezdete nem lehet korábbi, mint holnap
            if (berles.StartDate.Date < DateTime.Today.AddDays(1))
            {
                return BadRequest("The yacht rental can start no earlier than tomorrow.");
            }

            // Legalább 5 napos bérlés
            var napok = (berles.EndDate - berles.StartDate).TotalDays + 1;
            if (napok < 5)
            {
                return BadRequest("The minimum yacht rental period is 5 days.");
            }

            // Legfeljebb 90 napos bérlés
            if (napok > 90)
            {
                return BadRequest("The maximum yacht rental period is 90 days.");
            }

            // Átfedés ellenőrzése ugyanarra az irodára
            bool overlapExists = await _context.Berles.AnyAsync(b =>
                b.YachtId == berles.YachtId &&
                !(berles.EndDate <= b.StartDate || berles.StartDate >= b.EndDate)
            );

            if (overlapExists)
            {
                return BadRequest("The selected yacht is already booked during the specified period.");
            }

            _context.Berles.Add(berles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBerles", new { id = berles.Id }, berles);
        }

        // DELETE: api/Berlesek/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBerles(int id)
        {
            var berles = await _context.Berles.FindAsync(id);
            if (berles == null)
            {
                return NotFound();
            }

            _context.Berles.Remove(berles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BerlesExists(int id)
        {
            return _context.Berles.Any(e => e.Id == id);
        }
    }
}

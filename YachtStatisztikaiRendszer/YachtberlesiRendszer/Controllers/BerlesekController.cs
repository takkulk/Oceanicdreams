namespace YachtberlesiRendszer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class BerlesekController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BerlesekController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/berlesek
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Berles>>> GetAll()
        {
            return await _context.Berlesek.ToListAsync();
        }

        // GET /api/berlesek/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Berles>> GetById(int id)
        {
            var berles = await _context.Berlesek.FindAsync(id);
            if (berles == null)
                return NotFound();
            return berles;
        }

        // POST /api/berlesek
        [HttpPost]
        public async Task<ActionResult<Berles>> Create(Berles newBerles)
        {
            // Üzleti szabályok ellenőrzése
            var today = DateTime.Today;
            if (newBerles.StartDate.Date < today.AddDays(1))
                return BadRequest("A bérlés kezdőnapja nem lehet korábbi, mint a holnapi nap.");

            var durationDays = (newBerles.EndDate - newBerles.StartDate).TotalDays + 1;
            if (durationDays < 5)
                return BadRequest("A bérlés minimális időtartama 5 nap.");
            if (durationDays > 90)
                return BadRequest("A bérlés maximális időtartama 90 nap.");

            // Átfedés ellenőrzése ugyanarra a yachtra
            var overlapping = await _context.Berlesek.AnyAsync(b =>
                b.YachtId == newBerles.YachtId &&
                ((newBerles.StartDate >= b.StartDate && newBerles.StartDate <= b.EndDate) ||
                 (newBerles.EndDate >= b.StartDate && newBerles.EndDate <= b.EndDate) ||
                 (newBerles.StartDate <= b.StartDate && newBerles.EndDate >= b.EndDate))
            );

            if (overlapping)
                return BadRequest("Az adott yacht már foglalva az adott időszakban.");

            _context.Berlesek.Add(newBerles);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newBerles.Id }, newBerles);
        }

        // DELETE /api/berlesek/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var berles = await _context.Berlesek.FindAsync(id);
            if (berles == null)
                return NotFound();

            _context.Berlesek.Remove(berles);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

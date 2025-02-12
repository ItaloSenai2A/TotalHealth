using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHealth.Data;

namespace TotalHealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescricoesController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public PrescricoesController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Prescricoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescricao>>> GetPrescricoes()
        {
            return await _context.Prescricoes.ToListAsync();
        }

        // GET: api/Prescricoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prescricao>> GetPrescricao(Guid id)
        {
            var prescricao = await _context.Prescricoes.FindAsync(id);

            if (prescricao == null)
            {
                return NotFound();
            }

            return prescricao;
        }

        // PUT: api/Prescricoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescricao(Guid id, Prescricao prescricao)
        {
            if (id != prescricao.PrescricaoId)
            {
                return BadRequest();
            }

            _context.Entry(prescricao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescricaoExists(id))
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

        // POST: api/Prescricoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Prescricao>> PostPrescricao(Prescricao prescricao)
        {
            _context.Prescricoes.Add(prescricao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrescricao", new { id = prescricao.PrescricaoId }, prescricao);
        }

        // DELETE: api/Prescricoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescricao(Guid id)
        {
            var prescricao = await _context.Prescricoes.FindAsync(id);
            if (prescricao == null)
            {
                return NotFound();
            }

            _context.Prescricoes.Remove(prescricao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrescricaoExists(Guid id)
        {
            return _context.Prescricoes.Any(e => e.PrescricaoId == id);
        }
    }
}

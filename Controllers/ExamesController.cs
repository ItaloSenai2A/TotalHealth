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
    public class ExamesController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public ExamesController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Exames
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exame>>> GetExames()
        {
            return await _context.Exames.ToListAsync();
        }

        // GET: api/Exames/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exame>> GetExame(Guid id)
        {
            var exame = await _context.Exames.FindAsync(id);

            if (exame == null)
            {
                return NotFound();
            }

            return exame;
        }

        // GET: api/Exames/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Exame>>> GetExamesByNome(string nome)
        {
            var exames = await _context.Exames
                .Where(e => e.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (exames == null || !exames.Any())
            {
                return NotFound();
            }

            return exames;
        }

        // GET: api/Exames/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Exame>>> GetExamesByUsuario(Guid usuarioId)
        {
            var exames = await _context.Exames
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();

            if (exames == null || !exames.Any())
            {
                return NotFound();
            }

            return exames;
        }

        // GET: api/Exames/prescricao/{prescricaoId}
        [HttpGet("prescricao/{prescricaoId}")]
        public async Task<ActionResult<IEnumerable<Exame>>> GetExamesByPrescricao(Guid prescricaoId)
        {
            var exames = await _context.Exames
                .Where(e => e.PrescricaoId == prescricaoId)
                .ToListAsync();

            if (exames == null || !exames.Any())
            {
                return NotFound();
            }

            return exames;
        }

        // PUT: api/Exames/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExame(Guid id, Exame exame)
        {
            if (id != exame.ExameId)
            {
                return BadRequest();
            }

            _context.Entry(exame).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExameExists(id))
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


        // POST: api/Exames
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Exame>> PostExame(Exame exame)
        {
            _context.Exames.Add(exame);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExame", new { id = exame.ExameId }, exame);
        }

        // DELETE: api/Exames/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExame(Guid id)
        {
            var exame = await _context.Exames.FindAsync(id);
            if (exame == null)
            {
                return NotFound();
            }

            _context.Exames.Remove(exame);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExameExists(Guid id)
        {
            return _context.Exames.Any(e => e.ExameId == id);
        }
    }
}

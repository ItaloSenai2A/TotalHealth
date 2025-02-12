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
    public class MedicosEspecialidadesController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public MedicosEspecialidadesController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/MedicosEspecialidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicoEspecialidade>>> GetMedicoEspecialidades()
        {
            return await _context.MedicoEspecialidades.ToListAsync();
        }

        // GET: api/MedicosEspecialidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicoEspecialidade>> GetMedicoEspecialidade(Guid id)
        {
            var medicoEspecialidade = await _context.MedicoEspecialidades.FindAsync(id);

            if (medicoEspecialidade == null)
            {
                return NotFound();
            }

            return medicoEspecialidade;
        }

        // PUT: api/MedicosEspecialidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicoEspecialidade(Guid id, MedicoEspecialidade medicoEspecialidade)
        {
            if (id != medicoEspecialidade.MedicoEspecialidadeId)
            {
                return BadRequest();
            }

            _context.Entry(medicoEspecialidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicoEspecialidadeExists(id))
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

        // POST: api/MedicosEspecialidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicoEspecialidade>> PostMedicoEspecialidade(MedicoEspecialidade medicoEspecialidade)
        {
            _context.MedicoEspecialidades.Add(medicoEspecialidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicoEspecialidade", new { id = medicoEspecialidade.MedicoEspecialidadeId }, medicoEspecialidade);
        }

        // DELETE: api/MedicosEspecialidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicoEspecialidade(Guid id)
        {
            var medicoEspecialidade = await _context.MedicoEspecialidades.FindAsync(id);
            if (medicoEspecialidade == null)
            {
                return NotFound();
            }

            _context.MedicoEspecialidades.Remove(medicoEspecialidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicoEspecialidadeExists(Guid id)
        {
            return _context.MedicoEspecialidades.Any(e => e.MedicoEspecialidadeId == id);
        }
    }
}

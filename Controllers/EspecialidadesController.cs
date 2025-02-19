using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHealth.Data;
using TotalHealth.Models;

namespace TotalHealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EspecialidadesController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public EspecialidadesController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Especialidades
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Especialidade>>> GetEspecialidades()
        {
            return await _context.Especialidades.ToListAsync();
        }

        // GET: api/Especialidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Especialidade>> GetEspecialidade(Guid id)
        {
            var especialidade = await _context.Especialidades.FindAsync(id);

            if (especialidade == null)
            {
                return NotFound();
            }

            return especialidade;
        }
        // GET: api/Especialidades/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Especialidade>>> GetEspecialidadesByNome(string nome)
        {
            var especialidades = await _context.Especialidades
                .Where(e => e.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (especialidades == null || !especialidades.Any())
            {
                return NotFound();
            }

            return especialidades;
        }

        // GET: api/Especialidades/medico/{medicoId}
        [HttpGet("medico/{medicoId}")]
        public async Task<ActionResult<IEnumerable<Especialidade>>> GetEspecialidadesByMedico(Guid medicoId)
        {
            var especialidades = await _context.MedicoEspecialidades
                .Where(me => me.MedicoId == medicoId)
                .Select(me => me.Especialidade)
                .ToListAsync();

            if (especialidades == null || !especialidades.Any())
            {
                return NotFound();
            }

            return especialidades;
        }


        // PUT: api/Especialidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEspecialidade(Guid id, Especialidade especialidade)
        {
            if (id != especialidade.EspecialidadeId)
            {
                return BadRequest();
            }

            _context.Entry(especialidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspecialidadeExists(id))
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

        // POST: api/Especialidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Especialidade>> PostEspecialidade(Especialidade especialidade)
        {
            _context.Especialidades.Add(especialidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEspecialidade", new { id = especialidade.EspecialidadeId }, especialidade);
        }

        // DELETE: api/Especialidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEspecialidade(Guid id)
        {
            var especialidade = await _context.Especialidades.FindAsync(id);
            if (especialidade == null)
            {
                return NotFound();
            }

            _context.Especialidades.Remove(especialidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EspecialidadeExists(Guid id)
        {
            return _context.Especialidades.Any(e => e.EspecialidadeId == id);
        }
    }
}

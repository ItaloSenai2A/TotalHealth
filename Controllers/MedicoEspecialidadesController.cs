using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHealth.Data;
using TotalHealth.Models;

namespace TotalHealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoEspecialidadesController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public MedicoEspecialidadesController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/MedicoEspecialidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicoEspecialidade>>> GetMedicoEspecialidades()
        {
            return await _context.MedicoEspecialidades.ToListAsync();
        }

        // GET: api/MedicoEspecialidades/5
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
        // GET: api/MedicosEspecialidades/medico/{medicoId}
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

        // GET: api/MedicosEspecialidades/especialidade/{especialidadeId}
        [HttpGet("especialidade/{especialidadeId}")]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedicosByEspecialidade(Guid especialidadeId)
        {
            var medicos = await _context.MedicoEspecialidades
                .Where(me => me.EspecialidadeId == especialidadeId)
                .Select(me => me.Medico)
                .ToListAsync();

            if (medicos == null || !medicos.Any())
            {
                return NotFound();
            }

            return medicos;
        }

        // GET: api/MedicosEspecialidades/medico/nome/{nome}
        [HttpGet("medico/nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Especialidade>>> GetEspecialidadesByNomeMedico(string nome)
        {
            var especialidades = await _context.MedicoEspecialidades
                .Where(me => me.Medico.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .Select(me => me.Especialidade)
                .ToListAsync();

            if (especialidades == null || !especialidades.Any())
            {
                return NotFound();
            }

            return especialidades;
        }

        // PUT: api/MedicoEspecialidades/5
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

        // POST: api/MedicoEspecialidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicoEspecialidade>> PostMedicoEspecialidade(MedicoEspecialidade medicoEspecialidade)
        {
            _context.MedicoEspecialidades.Add(medicoEspecialidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicoEspecialidade", new { id = medicoEspecialidade.MedicoEspecialidadeId }, medicoEspecialidade);
        }

        // DELETE: api/MedicoEspecialidades/5
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

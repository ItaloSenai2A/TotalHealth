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
    public class MedicosController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public MedicosController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Medicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedicos()
        {
            return await _context.Medicos.ToListAsync();
        }

        // GET: api/Medicos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medico>> GetMedico(Guid id)
        {
            var medico = await _context.Medicos.FindAsync(id);

            if (medico == null)
            {
                return NotFound();
            }

            return medico;
        } // GET: api/Medicos/nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedicosByNome(string nome)
        {
            var medicos = await _context.Medicos
                .Where(m => m.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (medicos == null || !medicos.Any())
            {
                return NotFound();
            }

            return medicos;
        }

        // GET: api/Medicos/especialidade/{especialidadeId}
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

        // GET: api/Medicos/consulta/{consultaId}
        [HttpGet("consulta/{consultaId}")]
        public async Task<ActionResult<Medico>> GetMedicoByConsulta(Guid consultaId)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Medico)
                .FirstOrDefaultAsync(c => c.ConsultaId == consultaId);

            if (consulta == null)
            {
                return NotFound();
            }

            return consulta.Medico;
        }



        // PUT: api/Medicos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedico(Guid id, Medico medico)
        {
            if (id != medico.MedicoId)
            {
                return BadRequest();
            }

            _context.Entry(medico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicoExists(id))
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

        // POST: api/Medicos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Medico>> PostMedico(Medico medico)
        {
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedico", new { id = medico.MedicoId }, medico);
        }

        // DELETE: api/Medicos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedico(Guid id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicoExists(Guid id)
        {
            return _context.Medicos.Any(e => e.MedicoId == id);
        }
    }
}

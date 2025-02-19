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

    public class ConsultasController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public ConsultasController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Consultas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultas()
        {
            return await _context.Consultas.ToListAsync();
        }

        // GET: api/Consultas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> GetConsulta(Guid id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
            {
                return NotFound();
            }

            return consulta;
        }

        // GET: api/Consultas/date/{date}
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultasByDate(DateTime date)
        {
            var consultas = await _context.Consultas
                .Where(c => c.DataHora.Date == date.Date)
                .ToListAsync();

            if (consultas == null || !consultas.Any())
            {
                return NotFound();
            }

            return consultas;
        }

        // GET: api/Consultas/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultasByStatus(string status)
        {
            var consultas = await _context.Consultas
                .Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (consultas == null || !consultas.Any())
            {
                return NotFound();
            }

            return consultas;
        }

        // GET: api/Consultas/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultasByUsuario(Guid usuarioId)
        {
            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();

            if (consultas == null || !consultas.Any())
            {
                return NotFound();
            }

            return consultas;
        }

        // PUT: api/Consultas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsulta(Guid id, Consulta consulta)
        {
            if (id != consulta.ConsultaId) // Assuming the primary key is 'Id'
            {
                return BadRequest("Consulta ID mismatch.");
            }

            _context.Entry(consulta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaExists(id))
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

        // PATCH: api/Consultas/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateConsultaStatus(Guid id, [FromBody] string status)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            consulta.Status = status;
            _context.Entry(consulta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaExists(id))
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

        // POST: api/Consultas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Consulta>> PostConsulta(Consulta consulta)
        {
            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConsulta", new { id = consulta.ConsultaId }, consulta);
        }

        // DELETE: api/Consultas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta(Guid id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsultaExists(Guid id)
        {
            return _context.Consultas.Any(e => e.ConsultaId == id);
        }
    }
}

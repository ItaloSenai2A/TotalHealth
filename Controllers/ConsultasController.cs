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

        // POST: api/Consultas
        [HttpPost]
        public async Task<ActionResult<Consulta>> PostConsulta(Consulta consulta)
        {
            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsulta), new { id = consulta.ConsultaId }, consulta);
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
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
    public class ContatosController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public ContatosController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Contatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contato>>> GetContatos()
        {
            return await _context.Contatos.ToListAsync();
        }

        // GET: api/Contatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contato>> GetContato(Guid id)
        {
            var contato = await _context.Contatos.FindAsync(id);

            if (contato == null)
            {
                return NotFound();
            }

            return contato;
        }

        // GET: api/Contatos/tipo/{tipo}
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<Contato>>> GetContatosByTipo(string tipo)
        {
            var contatos = await _context.Contatos
                .Where(c => c.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (contatos == null || !contatos.Any())
            {
                return NotFound();
            }

            return contatos;
        }

        // GET: api/Contatos/valor/{valor}


        // GET: api/Contatos/endereco/{endereco}
        [HttpGet("endereco/{endereco}")]
        public async Task<ActionResult<IEnumerable<Contato>>> GetContatosByEndereco(string endereco)
        {
            var contatos = await _context.Contatos
                .Where(c => c.Endereco.Equals(endereco, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (contatos == null || !contatos.Any())
            {
                return NotFound();
            }

            return contatos;
        }


        // PUT: api/Contatos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContato(Guid id, Contato contato)
        {
            if (id != contato.ContatoId)
            {
                return BadRequest();
            }

            _context.Entry(contato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContatoExists(id))
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

        // POST: api/Contatos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contato>> PostContato(Contato contato)
        {
            _context.Contatos.Add(contato);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContato", new { id = contato.ContatoId }, contato);
        }

        // DELETE: api/Contatos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContato(Guid id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return NotFound();
            }

            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContatoExists(Guid id)
        {
            return _context.Contatos.Any(e => e.ContatoId == id);
        }
    }
}

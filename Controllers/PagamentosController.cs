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
    public class PagamentosController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public PagamentosController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Pagamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentos()
        {
            return await _context.Pagamentos.ToListAsync();
        }

        // GET: api/Pagamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> GetPagamento(Guid id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);

            if (pagamento == null)
            {
                return NotFound();
            }

            return pagamento;
        }
        // GET: api/Pagamentos/consulta/{consultaId}
        [HttpGet("consulta/{consultaId}")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentosByConsulta(Guid consultaId)
        {
            var pagamentos = await _context.Pagamentos
                .Where(p => p.ConsultaId == consultaId)
                .ToListAsync();

            if (pagamentos == null || !pagamentos.Any())
            {
                return NotFound();
            }

            return pagamentos;
        }

        // GET: api/Pagamentos/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentosByUsuario(Guid usuarioId)
        {
            var pagamentos = await _context.Pagamentos
                .Include(p => p.Consulta)
                .Where(p => p.Consulta.UsuarioId == usuarioId)
                .ToListAsync();

            if (pagamentos == null || !pagamentos.Any())
            {
                return NotFound();
            }

            return pagamentos;
        }

        // GET: api/Pagamentos/exame/{exameId}
        [HttpGet("exame/{exameId}")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentosByExame(Guid exameId)
        {
            var pagamentos = await _context.Pagamentos
                .Where(p => p.ExameId == exameId)
                .ToListAsync();

            if (pagamentos == null || !pagamentos.Any())
            {
                return NotFound();
            }

            return pagamentos;
        }


        // PUT: api/Pagamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagamento(Guid id, Pagamento pagamento)
        {
            if (id != pagamento.PagamentoId)
            {
                return BadRequest();
            }

            _context.Entry(pagamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagamentoExists(id))
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
        // PATCH: api/Pagamentos/{id}/valor
        [HttpPatch("{id}/valor")]
        public async Task<IActionResult> UpdatePagamentoValor(Guid id, [FromBody] decimal valor)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null)
            {
                return NotFound();
            }

            pagamento.Valor = valor;
            _context.Entry(pagamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagamentoExists(id))
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

        // POST: api/Pagamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pagamento>> PostPagamento(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPagamento", new { id = pagamento.PagamentoId }, pagamento);
        }

        // DELETE: api/Pagamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePagamento(Guid id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null)
            {
                return NotFound();
            }

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PagamentoExists(Guid id)
        {
            return _context.Pagamentos.Any(e => e.PagamentoId == id);
        }
    }
}

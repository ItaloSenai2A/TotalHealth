﻿using System;
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
    public class AgendamentosController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public AgendamentosController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Agendamentos
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Agendamento>>> GetAgendamentos()
        {
            return await _context.Agendamentos.ToListAsync();
        }

        // GET: api/Agendamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agendamento>> GetAgendamento(Guid id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);

            if (agendamento == null)
            {
                return NotFound();
            }

            return agendamento;
        }

        // PUT: api/Agendamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgendamento(Guid id, Agendamento agendamento)
        {
            if (id != agendamento.AgendamentoId)
            {
                return BadRequest();
            }

            _context.Entry(agendamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgendamentoExists(id))
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

        // POST: api/Agendamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agendamento>> PostAgendamento(Agendamento agendamento)
        {
            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgendamento", new { id = agendamento.AgendamentoId }, agendamento);
        }

        // DELETE: api/Agendamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgendamento(Guid id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound();
            }

            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgendamentoExists(Guid id)
        {
            return _context.Agendamentos.Any(e => e.AgendamentoId == id);
        }
        // GET: api/Agendamentos/date/{date}
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Agendamento>>> GetAgendamentosByDate(DateTime date)
        {
            var agendamentos = await _context.Agendamentos
                .Where(a => a.DataHora.Date == date.Date)
                .ToListAsync();

            if (agendamentos == null || !agendamentos.Any())
            {
                return NotFound();
            }

            return agendamentos;
        }

        // GET: api/Agendamentos/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Agendamento>>> GetAgendamentosByStatus(string status)
        {
            var agendamentos = await _context.Agendamentos
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (agendamentos == null || !agendamentos.Any())
            {
                return NotFound();
            }

            return agendamentos;
        }

    }
}

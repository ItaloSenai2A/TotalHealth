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
    public class UsuariosController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public UsuariosController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(Guid id, Usuario usuario)
        {
            if (id != usuario.UsuarioId) // Assuming the primary key is 'Id'
            {
                return BadRequest("User ID mismatch.");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioId }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(Guid id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
        // Autenticação de Usuário


        // Filtragem e Pesquisa de Usuários
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Usuario>>> SearchUsuarios(string nome, string email, string cpf)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(u => u.Nome.Contains(nome));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(cpf))
            {
                query = query.Where(u => u.Cpf.Contains(cpf));
            }

            return await query.ToListAsync();
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosPaged(int pageNumber = 1, int pageSize = 10)
        {
            var usuarios = await _context.Usuarios
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(usuarios);
        }

        // Recuperação de Dados Relacionados
        [HttpGet("{id}/consultas")]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultasByUsuario(Guid id)
        {
            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == id)
                .ToListAsync();

            if (consultas == null || !consultas.Any())
            {
                return NotFound();
            }

            return Ok(consultas);
        }

        [HttpGet("{id}/exames")]
        public async Task<ActionResult<IEnumerable<Exame>>> GetExamesByUsuario(Guid id)
        {
            var exames = await _context.Exames
                .Where(e => e.UsuarioId == id)
                .ToListAsync();

            if (exames == null || !exames.Any())
            {
                return NotFound();
            }

            return Ok(exames);
        }
    }
}
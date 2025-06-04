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
    [Authorize] // Protege todos os endpoints com autenticação JWT
    public class UsuariosLoginController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;

        public UsuariosLoginController(TotalHealthDBContext context)
        {
            _context = context;
        }

        // GET: api/UsuariosLogin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioLogin>>> GetUsuariosLogin()
        {
            return await _context.UsuariosLogin.ToListAsync();
        }

        // GET: api/UsuariosLogin/5
        [HttpGet("{email}")]
        public async Task<ActionResult<UsuarioLogin>> GetUsuarioLogin(string email)
        {
            var user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                return NoContent();
            }

            var userId = user.Id;

            if (!string.IsNullOrEmpty(userId))
            {
                var usuario = await _context.UsuariosLogin
                    .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

                if (usuario != null)
                {
                    return Ok(usuario);
                }
            }

            return NoContent();
        }

        // PUT: api/UsuariosLogin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarioLogin(Guid id, UsuarioLogin usuarioLogin)
        {
            if (id != usuarioLogin.UsuarioLoginId)
            {
                return BadRequest();
            }

            _context.Entry(usuarioLogin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioLoginExists(id))
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

        // POST: api/UsuariosLogin
        [HttpPost]
        public async Task<ActionResult<UsuarioLogin>> PostUsuarioLogin(UsuarioLogin usuarioLogin)
        {
            // Obter UserId do token JWT
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Buscar usuário no Identity
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return BadRequest("Usuário não encontrado no Identity.");
            }

            // Verificar se já existe um UsuarioLogin para este UserId
            var existingLogin = await _context.UsuariosLogin
                .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

            if (existingLogin != null)
            {
                return BadRequest("Este usuário já possui um login associado.");
            }

            // Associar o IdentityUser ao UsuarioLogin
            usuarioLogin.UserId = userId;
            usuarioLogin.User = user;

            _context.UsuariosLogin.Add(usuarioLogin);
            await _context.SaveChangesAsync();

            return Ok(usuarioLogin);
        }

        // DELETE: api/UsuariosLogin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioLogin(Guid id)
        {
            var usuarioLogin = await _context.UsuariosLogin.FindAsync(id);
            if (usuarioLogin == null)
            {
                return NotFound();
            }

            _context.UsuariosLogin.Remove(usuarioLogin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioLoginExists(Guid id)
        {
            return _context.UsuariosLogin.Any(e => e.UsuarioLoginId == id);
        }
    }
}

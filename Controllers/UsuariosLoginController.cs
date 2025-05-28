using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotalHealth.Data;
using TotalHealth.Migrations;
using TotalHealth.Models;

namespace TotalHealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

            // Pegar o Id da IdentityUser
            var userId = user.Id;

            // Buscar em usuarioLogin um IdentityUser associado pelo userId
            if (userId != null || userId != "")
            {
                var usuario = await _context.UsuariosLogin
                    .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
                if (usuario != null)
                {
                    return Ok(usuario);

                }
            }
            return BadRequest(new { message = "Usuário associado não encontrado." });
        }

        // PUT: api/UsuariosLogin/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioLogin>> PostUsuarioLogin(UsuarioLogin usuarioLogin)
        {
            _context.UsuariosLogin.Add(usuarioLogin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarioLogin", new { id = usuarioLogin.UsuarioLoginId }, usuarioLogin);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TotalHealth.Data;
using TotalHealth.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TotalHealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly TotalHealthDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsuariosController(TotalHealthDBContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: api/Usuarios
        [HttpGet]
        [AllowAnonymous]
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
            if (id != usuario.UsuarioId)
            {
                return BadRequest();
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized("Usuário não encontrado");
            }

            usuario.UserId = Guid.Parse(userId);
            usuario.Email = user.Email;
            usuario.Senha = user.PasswordHash;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.UsuarioId }, usuario);
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

        // POST: api/Usuarios/CompleteRegistration
        [HttpPost("CompleteRegistration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] CompleteRegistrationModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var usuario = new Usuario
            {
                UsuarioId = Guid.NewGuid(),
                Nome = model.Nome,
                Email = user.Email,
                Telefone = model.Telefone,
                Cpf = model.Cpf,
                Genero = model.Genero,
                TipoSanguineo = model.TipoSanguineo,
                Endereco = model.Endereco,
                UserId = Guid.Parse(user.Id),
                User = user
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        // POST: api/Usuarios/registrar
        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest("Já existe um usuário com este e-mail.");
            }

            var identityUser = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(identityUser, model.Senha);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var usuario = new Usuario
            {
                UsuarioId = Guid.NewGuid(),
                Nome = model.Nome,
                Email = model.Email,
                UserId = Guid.Parse(identityUser.Id),
                User = identityUser
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        // POST: api/Usuarios/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, model.Senha);
            if (!validPassword)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        private bool UsuarioExists(Guid id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }

        public class CompleteRegistrationModel
        {
            public string UserId { get; set; }
            public string Nome { get; set; }
            public string Telefone { get; set; }
            public string Cpf { get; set; }
            public string Genero { get; set; }
            public string TipoSanguineo { get; set; }
            public string Endereco { get; set; }
        }

        public class RegistroModel
        {
            public string Nome { get; set; }
            public string Email { get; set; }
            public string Senha { get; set; }
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Senha { get; set; }
        }
    }
}

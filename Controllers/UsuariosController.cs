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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(Guid id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest("O ID do usuário não confere com o corpo da requisição.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || usuario.UserId.ToString() != userId)
            {
                return Unauthorized("Você só pode atualizar seu próprio perfil.");
            }

            var usuarioExistente = await _context.Usuarios.Include(u => u.User).FirstOrDefaultAsync(u => u.UsuarioId == id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Atualiza os dados da tabela Usuarios
            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Telefone = usuario.Telefone;
            usuarioExistente.Cpf = usuario.Cpf;
            usuarioExistente.Genero = usuario.Genero;
            usuarioExistente.TipoSanguineo = usuario.TipoSanguineo;
            usuarioExistente.Endereco = usuario.Endereco;

            // Atualiza o IdentityUser (email)
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser != null)
            {
                if (identityUser.Email != usuario.Email)
                {
                    identityUser.Email = usuario.Email;
                    identityUser.UserName = usuario.Email;
                    identityUser.NormalizedEmail = usuario.Email.ToUpper();
                    identityUser.NormalizedUserName = usuario.Email.ToUpper();

                    var result = await _userManager.UpdateAsync(identityUser);
                    if (!result.Succeeded)
                    {
                        return BadRequest("Erro ao atualizar o e-mail do Identity.");
                    }
                }

                usuarioExistente.Email = identityUser.Email;
            }

            try
            {
                _context.Usuarios.Update(usuarioExistente);
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

            return Ok(usuarioExistente);
        }

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

        [HttpPost("CompleteRegistration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] CompleteRegistrationModel model)
        {
            if (!Guid.TryParse(model.UserId, out Guid userGuid))
            {
                return BadRequest("Formato de UserId inválido.");
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
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
                UserId = userGuid,
                User = user
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

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
                Cpf = model.Cpf,
                Genero = model.Genero,
                Endereco = model.Endereco,
                Telefone = model.Telefone,
                TipoSanguineo = model.TipoSanguineo,
                UserId = Guid.Parse(identityUser.Id),
                User = identityUser
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

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
            public string Cpf { get; set; }
            public string Genero { get; set; }
            public string Endereco { get; set; }
            public string Telefone { get; set; }
            public string TipoSanguineo { get; set; }
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Senha { get; set; }
        }
    }
}

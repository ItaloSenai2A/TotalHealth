using Microsoft.AspNetCore.Identity;

namespace TotalHealth.Models
{
    public class UsuarioLogin
    {
        public Guid UsuarioLoginId { get; set; }
        public string Cargo { get; set; }
        public string Username { get; set; }
        public string Telefone { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}

namespace TotalHealth.Models
{
    public class Usuario
    {
        public Guid UsuarioId { get; set; }
        public string? Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string? Telefone { get; set; }

    }
}

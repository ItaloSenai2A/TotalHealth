using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHealth.Models
{
    public class Consulta
    {
        public Guid ConsultaId { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public Guid MedicoId { get; set; }
        public Medico? Medico { get; set; }
        public DateTime DataHora { get; set; }
        public string Status { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
    }
}

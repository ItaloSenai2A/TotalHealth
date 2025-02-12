namespace TotalHealth.Models
{
    public class Medico
    {
        public Guid MedicoId { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
        public ICollection<MedicoEspecialidade> MedicoEspecialidades { get; set; }
        public ICollection<Consulta> Consultas { get; set; }
        public ICollection<Agendamento> Agendamentos { get; set; }
    }
}

namespace TotalHealth.Models
{
    public class Especialidade
    {
        public Guid EspecialidadeId { get; set; }
        public string Nome { get; set; }
        public ICollection<MedicoEspecialidade> MedicoEspecialidades { get; set; }
        public ICollection<Consulta> Consultas { get; set; }
        public ICollection<Agendamento> Agendamentos { get; set; }
    }
}

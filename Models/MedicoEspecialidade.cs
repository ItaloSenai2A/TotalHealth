using TotalHealth.Models;

public class MedicoEspecialidade
{
    public Guid MedicoEspecialidadeId { get; set; }
    public Guid MedicoId { get; set; }
    public Medico Medico { get; set; }
    public Guid EspecialidadeId { get; set; }
    public Especialidade Especialidade { get; set; }
}

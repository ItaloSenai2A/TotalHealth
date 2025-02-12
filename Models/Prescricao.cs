using TotalHealth.Models;

public class Prescricao
{
    public Guid PrescricaoId { get; set; }
    public Guid ConsultaId { get; set; }
    public Consulta Consulta { get; set; }
    public string Descricao { get; set; }
    public ICollection<Exame> Exames { get; set; }
}

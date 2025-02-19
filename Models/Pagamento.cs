using System.ComponentModel.DataAnnotations.Schema;
using TotalHealth.Models;

public class Pagamento
{
    public Guid PagamentoId { get; set; }
    public Guid ConsultaId { get; set; }
    public Consulta? Consulta { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }
    public DateTime? DataPagamento { get; set; }
    public Guid ExameId { get; set; }
    public Exame? Exame { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? ValorExame { get; set; } = 0;
}

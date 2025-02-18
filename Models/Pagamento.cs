using TotalHealth.Models;

public class Pagamento
{
    public Guid PagamentoId { get; set; }
    public Guid ConsultaId { get; set; }
    public Consulta? Consulta { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataPagamento { get; set; }
    public Guid ExameId { get; set; }
    public Exame? Exame { get; set; }
    public decimal? ValorExame { get; set; } = 0;
}
public class Exame
{
    public Guid ExameId { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public Guid PrescricaoId { get; set; }
    public Prescricao Prescricao { get; set; }
    public decimal Valor { get; set; }
    public ICollection<Pagamento> Pagamentos { get; set; }
}

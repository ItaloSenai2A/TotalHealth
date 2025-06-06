﻿namespace TotalHealth.Models
{
    public class Agendamento
    {
        public Guid AgendamentoId { get; set; }
        public Guid ConsultaId { get; set; }
        public Consulta? Consulta { get; set; }
        public DateTime DataHora { get; set; } // Corrigido de Data para DataHora
        public Guid MedicoId { get; set; }
        public Medico? Medico { get; set; }
        public Guid EspecialidadeId { get; set; }
        public Especialidade? Especialidade { get; set; }
        public string Status { get; set; }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TotalHealth.Models;

namespace TotalHealth.Data
{
    public class TotalHealthDBContext : IdentityDbContext
    {
        public TotalHealthDBContext(DbContextOptions<TotalHealthDBContext> options) : base(options)
        {
        }

        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Especialidade> Especialidades { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Prescricao> Prescricoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Agendamento>().ToTable("Agendamentos");
            modelBuilder.Entity<Consulta>().ToTable("Consultas");
            modelBuilder.Entity<Contato>().ToTable("Contatos");
            modelBuilder.Entity<Especialidade>().ToTable("Especialidades");
            modelBuilder.Entity<Exame>().ToTable("Exames");
            modelBuilder.Entity<Medico>().ToTable("Medicos");
            modelBuilder.Entity<Pagamento>().ToTable("Pagamentos");
            modelBuilder.Entity<Prescricao>().ToTable("Prescricoes");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");

        }
    }
}

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
        public DbSet<MedicoEspecialidade> MedicoEspecialidades { get; set; }

        public DbSet<UsuarioLogin> UsuariosLogin { get; set; }

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
            modelBuilder.Entity<MedicoEspecialidade>().ToTable("MedicosEspecialidades");

            // Configurar comportamento de exclusão para evitar múltiplos caminhos de exclusão em cascata
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Medico)
                .WithMany()
                .HasForeignKey(a => a.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Consulta)
                .WithMany()
                .HasForeignKey(a => a.ConsultaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Especialidade)
                .WithMany()
                .HasForeignKey(a => a.EspecialidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Medico)
                .WithMany()
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exame>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exame>()
                .HasOne(e => e.Prescricao)
                .WithMany()
                .HasForeignKey(e => e.PrescricaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pagamento>()
                .HasOne(p => p.Consulta)
                .WithMany()
                .HasForeignKey(p => p.ConsultaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pagamento>()
                .HasOne(p => p.Exame)
                .WithMany()
                .HasForeignKey(p => p.ExameId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescricao>()
                .HasOne(p => p.Consulta)
                .WithMany()
                .HasForeignKey(p => p.ConsultaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicoEspecialidade>()
                .HasOne(me => me.Medico)
                .WithMany()
                .HasForeignKey(me => me.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicoEspecialidade>()
                .HasOne(me => me.Especialidade)
                .WithMany()
                .HasForeignKey(me => me.EspecialidadeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

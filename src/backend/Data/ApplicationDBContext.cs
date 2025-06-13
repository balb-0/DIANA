using backend.Models.Entities;
using backend.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Assessor> Assessores { get; set; }
        public DbSet<Ativo> Ativos { get; set; }
        public DbSet<Investimento> Investimentos { get; set; }
        public DbSet<HistoricoRiscoDeclarado> HistoricoRiscoDeclarado { get; set; }
        public DbSet<HistoricoRiscoCalculado> HistoricoRiscoCalculado { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações de enum para PostgreSQL
            modelBuilder.Entity<Cliente>()
                .Property(e => e.Categoria)
                .HasConversion<string>();

            modelBuilder.Entity<HistoricoRiscoDeclarado>()
                .Property(e => e.Risco)
                .HasConversion<string>();

            modelBuilder.Entity<HistoricoRiscoCalculado>()
                .Property(e => e.Risco)
                .HasConversion<string>();

            // Relacionamentos
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Assessor)
                .WithMany(a => a.Clientes)
                .HasForeignKey(c => c.AssessoresId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Investimento>()
                .HasOne(i => i.Cliente)
                .WithMany(c => c.Investimentos)
                .HasForeignKey(i => i.ClientesId);

            modelBuilder.Entity<Investimento>()
                .HasOne(i => i.Ativo)
                .WithMany(a => a.Investimentos)
                .HasForeignKey(i => i.AtivosId);

            modelBuilder.Entity<HistoricoRiscoDeclarado>()
                .HasOne(h => h.Cliente)
                .WithMany(c => c.HistoricoRiscoDeclarado)
                .HasForeignKey(h => h.ClientesId);

            modelBuilder.Entity<HistoricoRiscoCalculado>()
                .HasOne(h => h.Cliente)
                .WithMany(c => c.HistoricoRiscoCalculado)
                .HasForeignKey(h => h.ClientesId);

            // Índices para performance (já que a lógica está no DB)
            modelBuilder.Entity<HistoricoRiscoDeclarado>()
                .HasIndex(h => new { h.ClientesId, h.CreatedAt });

            modelBuilder.Entity<HistoricoRiscoCalculado>()
                .HasIndex(h => new { h.ClientesId, h.CreatedAt });

            base.OnModelCreating(modelBuilder);
        }
    }
}
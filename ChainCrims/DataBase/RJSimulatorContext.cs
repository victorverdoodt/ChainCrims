using ChainCrims.Models;
using Microsoft.EntityFrameworkCore;

namespace ChainCrims.DataBase
{
    public class ChainCrimsContext : DbContext
    {
        public ChainCrimsContext(DbContextOptions<ChainCrimsContext> options) : base(options)
        {

        }

        public DbSet<Conta> Contas { get; set; }
        public DbSet<Arma> Armas { get; set; }
        public DbSet<Bonus> Bonus { get; set; }
        public DbSet<RegistroArma> RegistroArmas { get; set; }
        public DbSet<Registro> Registros { get; set; }
        public DbSet<RegistroSuspeito> RegistroSuspeitos { get; set; }
        public DbSet<Roubo> Roubos { get; set; }
        public DbSet<Suspeito> Suspeitos { get; set; }
        public DbSet<SuspeitoBonus> SuspeitoBonus { get; set; }
        public DbSet<ResultadoRoubo> ResultadoRoubos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}

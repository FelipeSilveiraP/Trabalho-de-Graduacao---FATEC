using LucroImpresso.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LucroImpresso.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Material> Materiais { get; set; }
        public DbSet<Orcamento> Orcamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Maquina> Maquinas { get; set; }
        public DbSet<ResumoFinanceiro> ResumoFinanceiro { get; set; }
    }
}
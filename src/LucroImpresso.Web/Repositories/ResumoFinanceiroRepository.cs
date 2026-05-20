using LucroImpresso.Web.Data;
using LucroImpresso.Web.Interfaces;
using LucroImpresso.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LucroImpresso.Web.Repositories
{
    public class ResumoFinanceiroRepository : IResumoFinanceiroRepository
    {
        private readonly AppDbContext _context;

        public ResumoFinanceiroRepository(AppDbContext context)
        {
            _context = context;
        }

        private async Task<ResumoFinanceiro> EnsureExistsAsync()
        {
            var resumo = await _context.ResumoFinanceiro.FirstOrDefaultAsync(r => r.Id == 1);
            if (resumo == null)
            {
                resumo = new ResumoFinanceiro { Id = 1, ReceitasAcumuladas = 0, DespesasMaterialAcumuladas = 0, DespesasEnergiaAcumuladas = 0 };
                _context.ResumoFinanceiro.Add(resumo);
                await _context.SaveChangesAsync();
            }
            return resumo;
        }

        public async Task<ResumoFinanceiro> GetResumoAsync()
        {
            return await EnsureExistsAsync();
        }

        public async Task AdicionarReceita(decimal valor)
        {
            var resumo = await EnsureExistsAsync();
            resumo.ReceitasAcumuladas += valor;
            _context.ResumoFinanceiro.Update(resumo);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarDespesaMaterial(decimal valor)
        {
            var resumo = await EnsureExistsAsync();
            resumo.DespesasMaterialAcumuladas += valor;
            _context.ResumoFinanceiro.Update(resumo);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarDespesaEnergia(decimal valor)
        {
            var resumo = await EnsureExistsAsync();
            resumo.DespesasEnergiaAcumuladas += valor;
            _context.ResumoFinanceiro.Update(resumo);
            await _context.SaveChangesAsync();
        }
    }
}

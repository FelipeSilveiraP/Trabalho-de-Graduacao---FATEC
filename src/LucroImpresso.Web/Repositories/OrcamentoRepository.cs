using LucroImpresso.Web.Data;
using LucroImpresso.Web.Interfaces;
using LucroImpresso.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LucroImpresso.Web.Repositories
{
    public class OrcamentoRepository : IOrcamentoRepository
    {
        private readonly AppDbContext _context;

        public OrcamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Orcamento>> GetAllAsync()
        {
            return await _context.Orcamentos.Include(o => o.Material).ToListAsync();
        }

        public async Task AddAsync(Orcamento orcamento)
        {
            await _context.Orcamentos.AddAsync(orcamento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Orcamento orcamento)
        {
            _context.Orcamentos.Update(orcamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var orcamento = await _context.Orcamentos.Include(o => o.Material).FirstOrDefaultAsync(o => o.Id == id);
            if (orcamento != null)
            {
                var resumo = await _context.ResumoFinanceiro.FirstOrDefaultAsync(r => r.Id == 1);
                if (resumo == null)
                {
                    resumo = new ResumoFinanceiro { Id = 1 };
                    _context.ResumoFinanceiro.Add(resumo);
                }

                if (orcamento.Status == StatusOrcamento.Concluido)
                {
                    resumo.ReceitasAcumuladas += orcamento.CustoFinal;
                }
                if (orcamento.Status == StatusOrcamento.Concluido || orcamento.Status == StatusOrcamento.Falha)
                {
                    resumo.DespesasEnergiaAcumuladas += (orcamento.ConsumoMaquinaW / 1000m) * orcamento.TempoHoras * orcamento.CustoKwh;
                }

                _context.Orcamentos.Remove(orcamento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetTotalFaturamentoAsync()
        {
            var orcamentos = await _context.Orcamentos
                .Include(o => o.Material)
                .Where(o => o.Status == StatusOrcamento.Concluido)
                .ToListAsync();
            
            var somaAtivos = orcamentos.Sum(o => o.CustoFinal);
            var resumo = await _context.ResumoFinanceiro.FirstOrDefaultAsync(r => r.Id == 1);
            return somaAtivos + (resumo?.ReceitasAcumuladas ?? 0);
        }

        public async Task<int> GetTotalPecasAsync()
        {
            return await _context.Orcamentos.CountAsync(o => o.Status == StatusOrcamento.Concluido);
        }

        public async Task<List<Orcamento>> GetUltimosOrcamentosAsync(int count)
        {
            return await _context.Orcamentos
                .Include(o => o.Material)
                .OrderByDescending(o => o.DataCriacao)
                .Take(count)
                .ToListAsync();
        }

        public async Task DeleteAntigosAsync(int dias)
        {
            var dataLimite = DateTime.Now.AddDays(-dias);
            var antigos = await _context.Orcamentos
                .Include(o => o.Material)
                .Where(o => o.DataCriacao < dataLimite)
                .ToListAsync();
            
            if (antigos.Any())
            {
                var resumo = await _context.ResumoFinanceiro.FirstOrDefaultAsync(r => r.Id == 1);
                if (resumo == null)
                {
                    resumo = new ResumoFinanceiro { Id = 1 };
                    _context.ResumoFinanceiro.Add(resumo);
                }

                foreach (var orcamento in antigos)
                {
                    if (orcamento.Status == StatusOrcamento.Concluido)
                    {
                        resumo.ReceitasAcumuladas += orcamento.CustoFinal;
                    }
                    if (orcamento.Status == StatusOrcamento.Concluido || orcamento.Status == StatusOrcamento.Falha)
                    {
                        resumo.DespesasEnergiaAcumuladas += (orcamento.ConsumoMaquinaW / 1000m) * orcamento.TempoHoras * orcamento.CustoKwh;
                    }
                }

                _context.Orcamentos.RemoveRange(antigos);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetTotalDespesasEnergiaAsync()
        {
            var impressos = await _context.Orcamentos
                .Where(o => o.Status == StatusOrcamento.Concluido || o.Status == StatusOrcamento.Falha)
                .ToListAsync();
            
            var somaAtivos = impressos.Sum(o => (o.ConsumoMaquinaW / 1000m) * o.TempoHoras * o.CustoKwh);
            var resumo = await _context.ResumoFinanceiro.FirstOrDefaultAsync(r => r.Id == 1);
            return somaAtivos + (resumo?.DespesasEnergiaAcumuladas ?? 0);
        }
    }
}

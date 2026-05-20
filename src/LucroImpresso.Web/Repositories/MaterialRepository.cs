using LucroImpresso.Web.Data;
using LucroImpresso.Web.Interfaces;
using LucroImpresso.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LucroImpresso.Web.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;

        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Material>> GetAllAsync()
        {
            // Continua lendo de forma limpa (sem sujar a memória desnecessariamente)
            return await _context.Materiais.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Material material)
        {
            // Limpa a memória por precaução antes de inserir
            _context.ChangeTracker.Clear();
            await _context.Materiais.AddAsync(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Material material)
        {
            // ==============================================================
            // A CORREÇÃO DE ARQUITETURA ESTÁ AQUI:
            // Limpa TODO o rastreamento antigo preso no circuito do Blazor
            // antes de mandar o EF atualizar o novo objeto.
            // ==============================================================
            _context.ChangeTracker.Clear(); 
            
            _context.Materiais.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Limpa o tracker e busca o objeto fresco direto do banco
            _context.ChangeTracker.Clear();
            
            var material = await _context.Materiais.FindAsync(id);
            if (material != null)
            {
                // COFRE ACUMULADOR: Salva a despesa do material antes de apagá-lo
                var resumo = await _context.ResumoFinanceiro.FirstOrDefaultAsync(r => r.Id == 1);
                if (resumo == null)
                {
                    resumo = new ResumoFinanceiro { Id = 1 };
                    _context.ResumoFinanceiro.Add(resumo);
                }
                resumo.DespesasMaterialAcumuladas += material.ValorPago;

                _context.Materiais.Remove(material);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetTotalDespesasMateriaisAsync()
        {
            var somaAtivos = await _context.Materiais.SumAsync(m => m.ValorPago);
            var resumo = await _context.ResumoFinanceiro.FirstOrDefaultAsync(r => r.Id == 1);
            return somaAtivos + (resumo?.DespesasMaterialAcumuladas ?? 0);
        }
    }
}
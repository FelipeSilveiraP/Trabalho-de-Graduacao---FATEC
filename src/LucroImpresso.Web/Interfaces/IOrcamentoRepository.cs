using LucroImpresso.Web.Models;

namespace LucroImpresso.Web.Interfaces
{
    public interface IOrcamentoRepository
    {
        Task<List<Orcamento>> GetAllAsync();
        Task AddAsync(Orcamento orcamento);
        Task UpdateAsync(Orcamento orcamento);
        Task DeleteAsync(int id);
        Task<decimal> GetTotalFaturamentoAsync();
        Task<int> GetTotalPecasAsync();
        Task<List<Orcamento>> GetUltimosOrcamentosAsync(int count);
        Task DeleteAntigosAsync(int dias);
        Task<decimal> GetTotalDespesasEnergiaAsync();
    }
}

using LucroImpresso.Web.Models;

namespace LucroImpresso.Web.Interfaces
{
    public interface IMaterialRepository
    {
        Task<List<Material>> GetAllAsync();
        Task AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task DeleteAsync(int id);
        Task<decimal> GetTotalDespesasMateriaisAsync();
    }
}
using LucroImpresso.Web.Models;

namespace LucroImpresso.Web.Interfaces
{
    public interface IMaquinaRepository
    {
        Task<List<Maquina>> GetAllAsync();
        Task AddAsync(Maquina maquina);
        Task UpdateAsync(Maquina maquina);
        Task DeleteAsync(int id);
    }
}

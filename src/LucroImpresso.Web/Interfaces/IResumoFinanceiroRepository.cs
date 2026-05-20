using LucroImpresso.Web.Models;

namespace LucroImpresso.Web.Interfaces
{
    public interface IResumoFinanceiroRepository
    {
        Task<ResumoFinanceiro> GetResumoAsync();
        Task AdicionarReceita(decimal valor);
        Task AdicionarDespesaMaterial(decimal valor);
        Task AdicionarDespesaEnergia(decimal valor);
    }
}

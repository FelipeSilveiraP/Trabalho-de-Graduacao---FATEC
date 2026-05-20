using LucroImpresso.Web.Models;

namespace LucroImpresso.Web.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> AutenticarAsync(string nomeUsuario, string senha);
        Task<bool> ExisteUsuarioAsync();
        Task CriarUsuarioPadraoAsync();
    }
}

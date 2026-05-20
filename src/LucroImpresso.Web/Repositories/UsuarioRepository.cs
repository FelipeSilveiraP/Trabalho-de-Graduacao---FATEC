using LucroImpresso.Web.Data;
using LucroImpresso.Web.Interfaces;
using LucroImpresso.Web.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace LucroImpresso.Web.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> AutenticarAsync(string nomeUsuario, string senha)
        {
            // 1. Busca o usuário apenas pelo nome
            var u = await _context.Usuarios.FirstOrDefaultAsync(x => x.NomeUsuario == nomeUsuario);
            
            // 2. Verifica se achou o usuário E se a senha digitada bate com o Hash salvo no banco
            if (u != null && BCrypt.Net.BCrypt.Verify(senha, u.Senha))
            {
                return u; // Autenticado com sucesso
            }
            
            return null; // Falha na autenticação (Usuário não existe ou senha incorreta)
        }

        public async Task<bool> ExisteUsuarioAsync()
        {
            return await _context.Usuarios.AnyAsync();
        }

        public async Task CriarUsuarioPadraoAsync()
        {
            // Cria o admin padrão APENAS se o banco estiver vazio
            if (!await ExisteUsuarioAsync())
            {
                // Regra de Ouro: Senha hasheada antes de ir pro banco!
                string senhaHasheada = BCrypt.Net.BCrypt.HashPassword("admin");
                
                await _context.Usuarios.AddAsync(new Usuario
                {
                    NomeUsuario = "admin",
                    Senha = senhaHasheada
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
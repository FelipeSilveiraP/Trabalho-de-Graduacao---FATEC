using LucroImpresso.Web.Data;
using LucroImpresso.Web.Interfaces;
using LucroImpresso.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LucroImpresso.Web.Repositories
{
    public class MaquinaRepository : IMaquinaRepository
    {
        private readonly AppDbContext _context;

        public MaquinaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Maquina>> GetAllAsync()
        {
            return await _context.Maquinas.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Maquina maquina)
        {
            _context.ChangeTracker.Clear();
            await _context.Maquinas.AddAsync(maquina);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Maquina maquina)
        {
            _context.ChangeTracker.Clear();
            _context.Maquinas.Update(maquina);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _context.ChangeTracker.Clear();
            var maquina = await _context.Maquinas.FindAsync(id);
            if (maquina != null)
            {
                _context.Maquinas.Remove(maquina);
                await _context.SaveChangesAsync();
            }
        }
    }
}

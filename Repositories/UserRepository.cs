
using api_econsulta.DB;
using api_econsulta.Interfaces;
using api_econsulta.Models;
using Microsoft.EntityFrameworkCore;


namespace api_econsulta.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EconsultaDbContext _context;

        public UserRepository(EconsultaDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllDoctorsAsync()
        {
            return await _context.Users
                .Where(u => u.Type == UserType.Doctor)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllPatientsAsync()
        {
            return await _context.Users
                .Where(u => u.Type == UserType.Patient)
                .ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
using System.Security.Cryptography;
using System.Text;
using DS_KnowledgeWebApi.Data;
using DS_KnowledgeWebApi.Models;
using DS_KnowledgeWebApi.ViewModels.UserViewModels;
using DS_KnowledgeWebApi.Сonstants;
using Microsoft.EntityFrameworkCore;

namespace DS_KnowledgeWebApi.Services
{
    public interface IAccountService
    {
        public Task<User?> Login(LoginUserViewModel loginUser);
        public Task<User?> Register(LoginUserViewModel loginUser);
    }

    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;

        public AccountService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> Login(LoginUserViewModel loginUser) =>
            await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u =>
                u.Email == loginUser.Email && u.Password == HaspPassword(loginUser.Password));

        public async Task<User?> Register(LoginUserViewModel loginUser)
        {
            var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            if (user != null) return null;
            await _db.Users.AddAsync(new User
            {
                Email = loginUser.Email,
                Password = HaspPassword(loginUser.Password),
                RoleId = (int)RolesConst.User
            });
            await _db.SaveChangesAsync();

            return await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == loginUser.Email);
        }

        private static string HaspPassword(string password)
        {
            var md5 = MD5.Create();
            var b = Encoding.ASCII.GetBytes(password);
            var hash = md5.ComputeHash(b);
            var sb = new StringBuilder();
            foreach (var a in hash)
                sb.Append(a.ToString("X2"));
            return sb.ToString();
        }
    }
}
using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public User GetByEmail(string email)
        {
            return context.Users
                .Include(user => user.CartLine)
                .ThenInclude(cartLine => cartLine.Products)
                .FirstOrDefault(item => item.Email == email);
        }

        public void Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
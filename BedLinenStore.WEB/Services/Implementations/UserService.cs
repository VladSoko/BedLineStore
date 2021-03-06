using System;
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

        public User GetById(int id)
        {
            return context.Users.FirstOrDefault(user => user.Id == id);
        }

        public User Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();

            return context.Users.FirstOrDefault(item => item.Email == user.Email);
        }

        public bool ConfirmEmail(User user, string email)
        {
            if (user.Email != email) return false;
            
            user.ConfirmedEmail = true;
            context.SaveChanges();
            return true;

        }

        public bool ResetPassword(User user, string password)
        {
            try
            {
                user.Password = password;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
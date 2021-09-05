using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IUserService
    {
        User GetByEmail(string email);

        User GetById(int id);

        User Create(User user);

        bool ConfirmEmail(User user, string email);

        bool ResetPassword(User user, string password);
    }
}
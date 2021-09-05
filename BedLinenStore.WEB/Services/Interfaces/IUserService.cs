using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IUserService
    {
        User GetByEmail(string email);

        void Create(User user);
    }
}
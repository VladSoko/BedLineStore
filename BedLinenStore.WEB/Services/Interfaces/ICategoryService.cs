using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface ICategoryService
    {
        Category GetById(int id);
    }
}
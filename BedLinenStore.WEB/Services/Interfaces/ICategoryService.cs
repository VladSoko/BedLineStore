using System.Collections.Generic;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface ICategoryService
    {
        Category GetById(int id);

        IEnumerable<Category> GetAll();
    }
}
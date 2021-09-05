using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IProductService
    {
        Product GetByMainInfoAndCategory(int mainInfoId, int categoryId);
    }
}
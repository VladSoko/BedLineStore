using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface ICartLineService
    {
        CartLine GetByEmail(string email);

        bool IsProductExist(CartLine cartLine, int productId);

        void AddProduct(CartLine cartLine, Product product);

        void DeleteProduct(CartLine cartLine, int productId);
    }
}
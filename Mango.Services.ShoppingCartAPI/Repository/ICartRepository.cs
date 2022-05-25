using Mango.Services.ShoppingCartAPI.Models.DTOs;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartDTO> GetCartByUserId(string userId);
        Task<CartDTO> CreateUpdateCart(CartDTO cartDTO);
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ClearCart(string userId);
    }
}

using Mango.Services.ShoppingCartAPI.Models.DTOs;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        public async Task<bool> ClearCart(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CartDTO> CreateUpdateCart(CartDTO cartDTO)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CartDTO> GetCartByUserId(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            throw new System.NotImplementedException();
        }
    }
}

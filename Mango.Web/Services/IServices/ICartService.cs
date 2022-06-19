using Mango.Web.Models;
using System.Threading.Tasks;

namespace Mango.Web.Services.IServices
{
    public interface ICartService
    {
        Task<T> GetCartByUserIdAsync<T>(string userid, string token = null);
        Task<T> AddToCartAsync<T>(CartDTO cartDTO, string token = null);
        Task<T> UpdateCartAsync<T>(CartDTO cartDTO, string token = null);
        Task<T> RemoveFromCartAsync<T>(int cartId, string token = null);
        Task<T> ApplyCouponAsync<T>(CartDTO cartDTO, string token = null);
        Task<T> RemoveCouponAsync<T>(string userId, string token = null);
        Task<T> ClearCartAsync<T>(string userId, string token = null);
        Task<T> Checkout<T>(CartHeaderDTO cartHeader, string token = null);
    }
}

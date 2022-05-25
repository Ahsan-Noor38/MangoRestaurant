using Mango.Web.Models;
using Mango.Web.Services.IServices;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory _clientFactory;
        public CartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> AddToCartAsync<T>(CartDTO cartDTO, string token = null)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDTO,
                Url = SD.ShoppingCartAPIBase + "/api/cart/AddCart",
                AccessToken = token
            };
            return await this.SendAsync<T>(apiRequest);
        }

        public async Task<T> ClearCartAsync<T>(string userId, string token = null)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = userId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/ClearCart",
                AccessToken = token
            };
            return await this.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userid, string token = null)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/cart/GetCart/" + userid,
                AccessToken = token
            };
            return await this.SendAsync<T>(apiRequest);
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart",
                AccessToken = token
            };
            return await this.SendAsync<T>(apiRequest);
        }

        public async Task<T> UpdateCartAsync<T>(CartDTO cartDTO, string token = null)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDTO,
                Url = SD.ShoppingCartAPIBase + "/api/cart/UpdateCart",
                AccessToken = token
            };
            return await this.SendAsync<T>(apiRequest);
        }
    }
}

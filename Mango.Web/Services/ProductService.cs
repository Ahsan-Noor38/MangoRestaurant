using Mango.Web.Models;
using Mango.Web.Services.IServices;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> CreateProductAsync<T>(ProductDTO product)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = product,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = ""
            };
            return await this.SendAsync<T>(apiRequest);
        }

        public async Task<T> DeleteProductAsync<T>(int id)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = ""
            };
            return await this.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAllProductsAsync<T>()
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = ""
            };
            return await this.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetProductByIdAsync<T>(int id)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = ""
            };
            return await this.SendAsync<T>(apiRequest);
        }

        public async Task<T> UpdateProductAsync<T>(ProductDTO product)
        {
            var apiRequest = new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = product,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = ""
            };
            return await this.SendAsync<T>(apiRequest);
        }
    }
}

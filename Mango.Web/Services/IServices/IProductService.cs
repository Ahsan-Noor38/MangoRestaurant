using Mango.Web.Models;
using System.Threading.Tasks;

namespace Mango.Web.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetProductByIdAsync<T>(int id);
        Task<T> CreateProductAsync<T>(ProductDTO product);
        Task<T> UpdateProductAsync<T>(ProductDTO product);
        Task<T> DeleteProductAsync<T>(int id);
    }
}

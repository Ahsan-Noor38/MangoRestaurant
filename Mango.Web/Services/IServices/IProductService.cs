﻿using Mango.Web.Models;
using System.Threading.Tasks;

namespace Mango.Web.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<T> GetAllProductsAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(int id, string token);
        Task<T> CreateProductAsync<T>(ProductDTO product, string token);
        Task<T> UpdateProductAsync<T>(ProductDTO product, string token);
        Task<T> DeleteProductAsync<T>(int id, string token);
    }
}

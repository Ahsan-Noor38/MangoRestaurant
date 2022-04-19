using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductDTO> CreateUpdateProduct(ProductDTO productDTO)
        {
            Product product = _mapper.Map<ProductDTO, Product>(productDTO);
            if (product.ProductId > 0)
            {
                _dbContext.Update(product);
            }
            else
            {
                _dbContext.Add(product);
            }
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _dbContext.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
                if (product == null)
                    return false;

                _dbContext.Remove(product);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<ProductDTO> GetProductById(int productId)
        {
            Product product = await _dbContext.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            List<Product> productList = await _dbContext.Products.ToListAsync();
            return _mapper.Map<List<ProductDTO>>(productList);
        }
    }
}

using AutoMapper;
using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Mango.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _dbContext;
        protected IMapper _mapper;
        public CouponRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<CouponDTO> GetCouponByCode(string couponCode)
        {
            var couponFromDb = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
            return _mapper.Map<CouponDTO>(couponFromDb);
        }
    }
}

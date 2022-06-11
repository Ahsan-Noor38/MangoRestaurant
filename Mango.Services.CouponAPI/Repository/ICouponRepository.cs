using Mango.Services.CouponAPI.Models.DTOs;
using System.Threading.Tasks;

namespace Mango.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCode(string couponCode);
    }
}

using Mango.Services.CouponAPI.Models.DTOs;
using Mango.Services.CouponAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDTO _response;

        public CouponAPIController(ICouponRepository couponRepository, ResponseDTO responseDTO)
        {
            _couponRepository = couponRepository;
            _response = responseDTO;
        }

        [HttpGet("{code}")]
        public async Task<object> GetCart(string code)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponByCode(code);
                _response.Result = coupon;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}

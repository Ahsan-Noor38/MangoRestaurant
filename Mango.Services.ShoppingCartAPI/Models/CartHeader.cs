using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CartHeaderid { get; set; }
        public string Userid { get; set; }
        public string CouponCode { get; set; }
    }
}

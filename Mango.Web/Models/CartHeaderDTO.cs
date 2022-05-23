namespace Mango.Web.Models
{
    public class CartHeaderDTO
    {
        public int CartHeaderid { get; set; }
        public string Userid { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotal { get; set; }
    }
}

namespace Mango.Services.OrderAPI.Messages
{
    public class CartDetailsDTO
    {
        public int CatDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }
        public int Count { get; set; }
    }
}

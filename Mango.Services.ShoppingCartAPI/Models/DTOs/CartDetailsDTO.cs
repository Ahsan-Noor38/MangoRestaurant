namespace Mango.Services.ShoppingCartAPI.Models.DTOs
{
    public class CartDetailsDTO
    {
        public int CatDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public virtual CartHeaderDTO CartHeader { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }
        public int Count { get; set; }
    }
}

using Mango.Services.OrderAPI.DbContexts;
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _dbContext = new ApplicationDbContext(_dbContextOptions);
            _dbContext.OrderHeaders.Add(orderHeader);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool isPaid)
        {
            await using var _dbContext = new ApplicationDbContext(_dbContextOptions);
            var orderHeaderFromDb = await _dbContext.OrderHeaders.FirstOrDefaultAsync(o => o.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = isPaid;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

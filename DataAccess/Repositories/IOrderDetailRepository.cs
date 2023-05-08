using BusinessObject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAll();
        Task<IEnumerable<OrderDetail>> Get(int? productId, int orderId);
        Task Add(OrderDetail orderDetail);
        Task Delete(int productId, int orderId);
        Task Update(OrderDetail orderDetail);
    }
}

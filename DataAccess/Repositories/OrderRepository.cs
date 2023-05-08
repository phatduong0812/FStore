using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public Task<IEnumerable<Order>> GetMany(int? memberId)
        {
            return OrderDAO.Instance.GetMany(memberId);
        }

        public Task<Order?> Get(int id)
        {
            return OrderDAO.Instance.Get(id);
        }

        public Task Add(Order order)
        {
            return OrderDAO.Instance.Add(order);
        }

        public Task Delete(int id)
        {
            return OrderDAO.Instance.Delete(id);
        }

        public Task Update(Order order)
        {
            return OrderDAO.Instance.Update(order);
        }
    }
}

using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetMany(string? queryKeyword);
        Task<Product?> Get(int id);
        Task Add(Product product);
        Task Delete(int id);
        Task Update(Product product);
    }
}

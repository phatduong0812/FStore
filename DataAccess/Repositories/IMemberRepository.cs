using BusinessObject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IMemberRepository
    {
        Task<Member> Authentication(string email, string password);
        Task<IEnumerable<Member>> GetAll();
        Task<Member?> Get(int id);
        Task Add(Member member);
        Task Delete(int id);
        Task Update(Member member);
    }
}

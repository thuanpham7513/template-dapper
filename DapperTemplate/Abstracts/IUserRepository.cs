using DapperTemplate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperTemplate.Abstracts
{
    public interface IUserRepository
    {
        Task<bool> Create(User user);
        Task<IEnumerable<User>> GetAll(int pageIndex, int pageSize);
        Task<User> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Update(User input);
    }
}

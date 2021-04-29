using DapperTemplate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperTemplate.Abstracts.Services
{
    public interface IUserService
    {
        Task<bool> Create(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(int id);
        Task<IEnumerable<UserResponseModel>> GetAll(int? pageIndex = null,
            int? pageSize = null,
            string search = null,
            bool? desc = null);
    }
}

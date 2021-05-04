using DapperTemplate.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperTemplate.Abstracts
{
    public interface IUserRepository
    {
        Task<bool> Create(User user);
        Task<IEnumerable<User>> GetAll(
            int? pageIndex = null,
            int? pageSize = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string searchEmail = null,
            string searchName = null,
            string searchAddress = null,
            bool? desc = null);
        Task<User> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Update(User input);
    }
}

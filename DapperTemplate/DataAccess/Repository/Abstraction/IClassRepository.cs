using DapperTemplate.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.DataAccess.Repository.Abstraction
{
    public interface IClassRepository
    {
        Task<bool> Create(Class user);
        Task<IEnumerable<Class>> GetAll(
            int? pageIndex = null,
            int? pageSize = null,
            string search = null,
            bool? desc = null);
        Task<Class> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Update(Class input);
    }
}

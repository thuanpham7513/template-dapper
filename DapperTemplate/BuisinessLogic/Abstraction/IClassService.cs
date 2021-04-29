using DapperTemplate.Models;
using DapperTemplate.Models.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperTemplate.BuisinessLogic.Abstraction
{
    public interface IClassService
    {
        Task<bool> Create(Class user);
        Task<bool> Update(Class user);
        Task<bool> Delete(int id);
        Task<IEnumerable<ClassResponseModel>> GetAll(int? pageIndex = null,
            int? pageSize = null,
            string search = null,
            bool? desc = null);
    }
}

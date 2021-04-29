using AutoMapper;
using DapperTemplate.BuisinessLogic.Abstraction;
using DapperTemplate.DataAccess.Repository.Abstraction;
using DapperTemplate.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.BuisinessLogic.Implementation
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;
        public ClassService(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }
        public async Task<bool> Create(Class user)
        {
            return await _classRepository.Create(user);
        }

        public async Task<bool> Delete(int id)
        {
            return await _classRepository.Delete(id);
        }

        public async Task<IEnumerable<ClassResponseModel>> GetAll(int? pageIndex = null, int? pageSize = null, string search = null, bool? desc = null)
        {
            var data = await _classRepository.GetAll(pageIndex, pageSize, search, desc);
            return _mapper.Map<IEnumerable<ClassResponseModel>>(data);
        }

        public async Task<bool> Update(Class user)
        {
            return await _classRepository.Update(user);
        }
    }
}

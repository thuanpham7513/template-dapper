using AutoMapper;
using DapperTemplate.Abstracts;
using DapperTemplate.Abstracts.Services;
using DapperTemplate.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperTemplate.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<bool> Create(User user)
        {
            return await _userRepository.Create(user);
        }

        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }

        public async Task<IEnumerable<UserResponseModel>> GetAll(
            int? pageIndex = null,
            int? pageSize = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string searchEmail = null,
            string searchName = null,
            string searchAddress = null,
            bool? desc = null)
        {
            var data =  await _userRepository.GetAll(
                pageIndex: pageIndex, 
                pageSize: pageSize,
                startDate: startDate,
                endDate: endDate,
                searchEmail: searchEmail, 
                searchName: searchName, 
                searchAddress: searchAddress, 
                desc: desc);
            return _mapper.Map<IEnumerable<UserResponseModel>>(data);
        }

        public async Task<bool> Update(User user)
        {
            return await _userRepository.Update(user);
        }
    }
}

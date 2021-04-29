using AutoMapper;
using DapperTemplate.Abstracts;
using DapperTemplate.Abstracts.Services;
using DapperTemplate.Models;
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

        public async Task<IEnumerable<UserResponseModel>> GetAll(int? pageIndex = null, int? pageSize = null, string search = null, bool? desc = null)
        {
            var data =  await _userRepository.GetAll(pageIndex, pageSize, search, desc);
            return _mapper.Map<IEnumerable<UserResponseModel>>(data);
        }

        public async Task<bool> Update(User user)
        {
            return await _userRepository.Update(user);
        }
    }
}

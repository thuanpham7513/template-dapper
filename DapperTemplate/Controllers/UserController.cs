using AutoMapper;
using DapperTemplate.Abstracts;
using DapperTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper; 

        public UserController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepository = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }
            var records = await _userRepository.GetAll(pageIndex, pageSize);


            return Ok(new UserPagingResponseModel 
            {
                PageIndex = pageIndex,
                Total = records.Count(),
                Records = _mapper.Map<IEnumerable<UserResponseModel>>(records)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<User>(requestModel);
            if(!(await _userRepository.Create(user)))
            {
                return StatusCode(500);
            }
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]UserRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<User>(requestModel);
            user.Id = id;

            if (!(await _userRepository.Update(user)))
            {
                return StatusCode(500);
            }

            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!(await _userRepository.Delete(id)))
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}

using AutoMapper;
using DapperTemplate.Abstracts;
using DapperTemplate.Abstracts.Services;
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
        private readonly IUserService _userService;
        private readonly IMapper _mapper; 

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize, string searchValue)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }
            var records = await _userService.GetAll(pageIndex, pageSize, desc: true, search: searchValue);


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
            if(!(await _userService.Create(user)))
            {
                return StatusCode(500);
            }
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UserRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<User>(requestModel);
            user.Id = id;

            if (!(await _userService.Update(user)))
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

            if (!(await _userService.Delete(id)))
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}

using AutoMapper;
using DapperTemplate.Abstracts.Services;
using DapperTemplate.Models;
using DapperTemplate.Models.QueryModels;
using DapperTemplate.Models.QueryParameters;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAll([FromQuery] QueryParameter query)
        {
            if (query.PageIndex <= 0 || query.PageSize <= 0)
            {
                return BadRequest();
            }

            var records = await _userService.GetAll(
                pageIndex: query.PageIndex,
                pageSize: query.PageSize,
                startDate: query.StartDate,
                endDate: query.EndDate,
                searchEmail: "string",
                searchName: "string");

            var result = new QueryResult<UserResponseModel>() 
            {
                PagingResult = new PagingResult<UserResponseModel>() 
                {
                    PageIndex = query.PageIndex,
                    PageSize = query.PageSize,
                    Records = records,
                    Total = records.FirstOrDefault().TotalCount
                }
            };

            return Ok(result);
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

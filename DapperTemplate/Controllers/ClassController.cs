using AutoMapper;
using DapperTemplate.BuisinessLogic.Abstraction;
using DapperTemplate.Models.Classes;
using DapperTemplate.Models.QueryModels;
using DapperTemplate.Models.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IMapper _mapper;

        public ClassController(IClassService userService, IMapper mapper)
        {
            _classService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameter query)
        {
            if (query.PageIndex <= 0 || query.PageSize <= 0)
            {
                return BadRequest();
            }

            var records = await _classService.GetAll(
                query.PageIndex,
                query.PageSize,
                desc: true,
                search: query.SearchValue);
            var result = new QueryResult<ClassResponseModel>()
            {
                PagingResult = new PagingResult<ClassResponseModel>()
                {
                    PageIndex = query.PageIndex,
                    PageSize = query.PageSize,
                    Records = records,
                    Total = records.First().Total
                }
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<Class>(requestModel);
            if (!(await _classService.Create(user)))
            {
                return StatusCode(500);
            }
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ClassRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<Class>(requestModel);
            user.Id = id;

            if (!(await _classService.Update(user)))
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

            if (!(await _classService.Delete(id)))
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}

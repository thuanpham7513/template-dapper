using AutoMapper;
using DapperTemplate.BuisinessLogic.Abstraction;
using DapperTemplate.Models.Classes;
using DapperTemplate.Models.QueryModels;
using DapperTemplate.Models.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public ClassController(IClassService userService, IMapper mapper, IDistributedCache distributedCache)
        {
            _classService = userService;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameter query)
        {
            if (query.PageIndex <= 0 || query.PageSize <= 0)
            {
                return BadRequest();
            }

            var cacheKey = "customerList";
            string serializedClassList;
            var records = new List<ClassResponseModel>();
            var redisCustomerList = await _distributedCache.GetAsync(cacheKey);
            if (redisCustomerList != null)
            {
                serializedClassList = Encoding.UTF8.GetString(redisCustomerList);
                records = JsonConvert.DeserializeObject<List<ClassResponseModel>>(serializedClassList);
            }
            else
            {
                records = (await _classService.GetAll(
                         query.PageIndex,
                         query.PageSize,
                         desc: true,
                         search: query.SearchValue)).ToList();
                serializedClassList = JsonConvert.SerializeObject(records);

                redisCustomerList = Encoding.UTF8.GetBytes(serializedClassList);

                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                await _distributedCache.SetAsync(cacheKey, redisCustomerList, options);
            }
           
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

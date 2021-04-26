using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.Models
{
    public class UserPagingResponseModel
    {
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public IEnumerable<UserResponseModel> Records { get; set; }
    }
}

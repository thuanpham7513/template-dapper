using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.Models.QueryParameters
{
    public class QueryParameter
    {
        [Range(1, 1000)]
        public int PageSize { get; set; }
        [Range(1, 1000)]
        public int PageIndex { get; set; }
        public string SearchValue { get; set; }
    }
}

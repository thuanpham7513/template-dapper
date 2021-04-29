using System.Collections.Generic;

namespace DapperTemplate.Models.QueryModels
{
    public class QueryResult<T>
    {
        public int Status { get; set; }
        public PagingResult<T> PagingResult { get; set; }
    }

    public class PagingResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Records { get; set; }
    }
}

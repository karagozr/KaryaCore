using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Karya.Core.Common.Data
{

    public class FilterDataOptions<TEntity>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool RequireTotalCount { get; set; }
        public Expression<Func<TEntity, bool>>? FilterExpression { get; set; }
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderByExpression { get; set; }
    }

    public class FilterDataResult<TData>
    {
        public IEnumerable<TData> Data { get; set; } = Array.Empty<TData>();
        public int TotalCount { get; set; }
    }
}

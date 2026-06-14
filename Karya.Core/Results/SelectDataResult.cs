using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karya.Core.Results;

public class SelectDataResult<TEntity>
{
    public IEnumerable<TEntity> Data { get; set; } = Array.Empty<TEntity>();
    public int TotalCount { get; set; }

}

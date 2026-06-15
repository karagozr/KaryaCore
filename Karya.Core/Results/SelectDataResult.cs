using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karya.Core.Results;

public class SelectDataResult<T>
{
    public IEnumerable<T> Data { get; set; } = Array.Empty<T>();
    public int TotalCount { get; set; }

}

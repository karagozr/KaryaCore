using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karya.Core.Helpers.Generals;

public static class EntityPropertyHelper
{
    public static bool HasProperty(this object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName) != null;
    }
}

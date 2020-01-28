using System;
using System.Collections.Generic;

namespace Vouzamo.ERM.Common
{
    public interface IHasProperties<T>
    {
        Guid Type { get; set; }
        Dictionary<string, T> Properties { get; set; }
    }
}

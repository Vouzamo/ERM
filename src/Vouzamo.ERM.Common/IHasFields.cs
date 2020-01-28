using System.Collections.Generic;

namespace Vouzamo.ERM.Common
{
    public interface IHasFields
    {
        List<Field> Fields { get; set; }
    }
}

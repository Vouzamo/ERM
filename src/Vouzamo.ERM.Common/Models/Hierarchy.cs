using System;
using System.Collections.Generic;
using System.Text;

namespace Vouzamo.ERM.Common.Models
{
    public class Hierarchy<T>
    {
        public T Source { get; set; }
        public List<Hierarchy<T>> Children { get; set; }

        public Hierarchy()
        {
            Children = new List<Hierarchy<T>>();
        }

        public Hierarchy(T subject) : this()
        {
            Source = subject;
        }
    }
}

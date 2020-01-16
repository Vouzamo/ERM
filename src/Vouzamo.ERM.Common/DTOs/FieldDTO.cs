using System;
using System.Collections.Generic;
using System.Text;

namespace Vouzamo.ERM.DTOs
{
    public class FieldDTO
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Mandatory { get; set; }
        public bool Enumerable { get; set; }
    }
}

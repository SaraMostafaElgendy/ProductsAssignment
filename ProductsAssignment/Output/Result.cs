using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAssignment
{
    public class Result
    {
        public bool success { get; set; }
        public object? results { get; set; }
        public string[] messages { get; set; } = new string[1];
    }
}

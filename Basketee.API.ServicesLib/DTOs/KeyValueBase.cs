using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs
{
    public abstract class KeyValueBase<T> where T : struct
    {
        public string key { get; set; }
        public T value { get; set; }
    }
}

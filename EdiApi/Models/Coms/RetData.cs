using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public class RetData<T>
    {
        public T Data { get; set; }
        public RetInfo Info { get; set; } = new RetInfo();
    }
}

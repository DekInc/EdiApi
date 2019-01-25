using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class EdiSec
    {
        public static int CheckSeg = 0;
        public string HashId { set; get; }
        public string EdiStr { set; get; } = "";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComModels
{
    public class RetData<T>
    {
        public T Data { get; set; }
        public RetInfo Info { get; set; } = new RetInfo();

        public static implicit operator RetData<T>(RetData<Tuple<IEnumerable<PaylessProdPrioriM>, IEnumerable<PaylessProdPrioriDet>>> v)
        {
            throw new NotImplementedException();
        }
    }
}

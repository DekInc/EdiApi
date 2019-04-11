using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class IenetGroupsAccesses
    {
        public int Id { get; set; }
        public int IdIenetGroup { get; set; }
        public int IdIenetAccess { get; set; }
    }
}

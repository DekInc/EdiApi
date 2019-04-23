using ComModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Models
{
    public class IenetGroupsModel : IenetGroups
    {
        public int recid { get { return Id; } }
    }
}

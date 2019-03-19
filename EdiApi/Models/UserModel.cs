using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    [Serializable]
    public class UserModel
    {
        public string User { set; get; }
        public string Password { set; get; }
    }
}

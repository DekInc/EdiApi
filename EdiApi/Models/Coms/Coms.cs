using EdiApi.Models.EdiDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class Coms
    {
        public static void AddComLog(ref EdiDBContext _DbO, string _Type, string _Log)
        {
            _DbO.EdiComs.Add(new EdiComs()
            {
                Type = _Type,
                Freg = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                Log = _Log
            });
            _DbO.SaveChanges();
        }
        public static void CheckMaxEdiComs(ref EdiDBContext _DbO, object _MaxEdiComs)
        {
            if (_DbO.EdiComs.Count() > Convert.ToInt32(_MaxEdiComs))
                foreach (EdiComs EdiComO in _DbO.EdiComs) _DbO.EdiComs.Remove(EdiComO);
        }
    }
}

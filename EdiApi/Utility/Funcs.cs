using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Utility {
    public class Funcs {
        public static IEnumerable<DateTime> AllThursdaysInMonth(int year, int month) {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++) {
                if ((new DateTime(year, month, day)).DayOfWeek == DayOfWeek.Thursday)
                    yield return new DateTime(year, month, day, 13, 0, 0);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EdiApi.Utility {
    public class Funcs {
        public static IEnumerable<DateTime> AllThursdaysInMonth(int year, int month) {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++) {
                if ((new DateTime(year, month, day)).DayOfWeek == DayOfWeek.Thursday)
                    yield return new DateTime(year, month, day, 15, 0, 0);
            }
        }
        public static IEnumerable<DateTime> AllSaturdayInMonth(int year, int month) {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++) {
                if ((new DateTime(year, month, day)).DayOfWeek == DayOfWeek.Saturday)
                    yield return new DateTime(year, month, day, 10, 0, 0);
            }
        }
        /// <summary>
        /// Copia todo lo del origen al destino, pero deben ser iguales
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="Or"></param>
        /// <param name="Dest"></param>
        /// <returns></returns>
        public static T2 Reflect<T1, T2>(T1 Or, T2 Dest) {
            foreach (PropertyInfo PropertyInfoO in Or.GetType().GetProperties()) {
                try {
                    if (Dest.GetType().GetProperty(PropertyInfoO.Name) != null && Dest.GetType().GetProperty(PropertyInfoO.Name).CanWrite)
                        Dest.GetType().GetProperty(PropertyInfoO.Name).SetValue(Dest, Or.GetType().GetProperty(PropertyInfoO.Name).GetValue(Or));
                } catch (Exception er1) {
                    throw er1;
                }
            }
            return Dest;
        }
    }
}

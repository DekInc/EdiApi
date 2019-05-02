using System;
using System.Collections.Generic;
using System.Reflection;

namespace EdiViewer.Utility {
    public static class Funcs {
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
        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month) {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++) {
                yield return new DateTime(year, month, day);
            }
        }
    }    
}

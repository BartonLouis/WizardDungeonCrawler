using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils {
    public static class EnumUtils {
        public static IEnumerable<T> GetValues<T>() where T : Enum {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool MoreThanOneFlag<T>(this T flag) where T : Enum { 
            return (Convert.ToInt32(flag) & (Convert.ToInt32(flag) - 1)) != 0;
        }

        public static int CountFlags<T>(this T val) where T : Enum {
            int count = 0;
            foreach (var flag in GetValues<T>()) {
                if (val.HasFlag(flag)) count++;
            }
            return count;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils {
    public static class EnumUtils {
        public static IEnumerable<T> GetValues<T>() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool MoreThanOneFlag<T>(T flag) where T : Enum { 
            return (Convert.ToInt32(flag) & (Convert.ToInt32(flag) - 1)) != 0;
        }
    }
}
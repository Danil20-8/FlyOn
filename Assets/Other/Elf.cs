using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public static class Elf
    {
        public static void With<T>(T variable, params Action<T>[] actions)
        {
            foreach (var a in actions)
                a(variable);
        }

        public static Tvalue SN<Tnull, Tvalue>(this Tnull nullable, Func<Tnull, Tvalue> getter, Tvalue defaultValue)
        {
            return nullable != null ? getter(nullable) : defaultValue;
        }
        public static Tnull SN<Tnull>(this Tnull nullable, Action<Tnull> setter)
        {
            if (nullable != null)
                setter(nullable);
            return nullable;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public static class UAssert
    {
        public static void NotNull(object obj)
        {
            if (obj == null)
                throw new Exception(obj.ToString() + " is null");
        }
    }
}

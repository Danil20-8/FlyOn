using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public class OneProperty<T>
    {
        T _value;
        public T value { get { return _value; } }
        public OneProperty(T value)
        {
            _value = value;
        }

        public static implicit operator T(OneProperty<T> self)
        {
            return self._value;
        }
    }
}

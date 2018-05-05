using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    [Serializable]
    public struct RangeValue<T> where T : IComparable
    {
        public T minValue { get { return _minValue; } set { if (value.CompareTo(_maxValue) > 0) _maxValue = value; _minValue = value; this.value = _value; } }
        public T maxValue { get { return _maxValue; } set { if (value.CompareTo(_minValue) < 0) _minValue = value; _maxValue = value; this.value = _value; } }

        T _minValue;
        T _maxValue;

        public T value { get { return _value; } set { if (value.CompareTo(_minValue) < 0) _value = _minValue; else if (value.CompareTo(_maxValue) > 0) _value = _maxValue; else _value = value; } }
        T _value;

        public RangeValue(T minValue, T maxValue, T value)
        {
            _minValue = default(T);
            _maxValue = default(T);
            _value = default(T);

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.value = value;
        }

        public static implicit operator T(RangeValue<T> value)
        {
            return value.value;
        }
    }
}

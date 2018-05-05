using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public struct CustomEnumerable<T> : IEnumerable<T>
    {
        IEnumerator<T> enumerator;
        public CustomEnumerable(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return enumerator;
        }
    }
}

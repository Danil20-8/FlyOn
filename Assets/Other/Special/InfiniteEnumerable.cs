using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other.Special
{
    /// <summary>
    /// Be careful!!!
    /// </summary>
    internal class InfiniteEnumerable<T>: IEnumerable<T>
    {
        Func<int, T> creator;
        public InfiniteEnumerable(Func<int, T> creator)
        {
            this.creator = creator;
        }
        public InfiniteEnumerable(Func<T> creator)
        {
            this.creator = i => creator();
        }
        public IEnumerator<T> GetEnumerator()
        {
            int i = 0;
            again:
            yield return creator(i);
            i++;
            goto again;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

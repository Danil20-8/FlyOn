using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Assets.Other.Special
{
    public class InfiniteStack<T>  : Stack where T: ICloneable<T>
    {
        public override int Count { get { return count; } }
        int count;
        object last;
        public InfiniteStack(T obj, int count = -1)
        {
            Push(obj);
            this.count = count;
        }

        public override void Push(object obj)
        {
            if (!(obj is T))
                throw new Exception();
            last = null;
            base.Clear();
            base.Push(obj);
        }
        public override object Pop()
        {
            var p = Get();
            last = null;
            return p;
        }
        public override object Peek()
        {
            return Get();
        }
        object Get()
        {
            if (last != null)
                return last;
            else {
                if (count == 0)
                    return null;
                else if(count > 0)
                    count--;
                if (count == 0)
                    return base.Pop();
                return last = ((T) base.Peek()).Clone();
            }
        }
    }


    public interface ICloneable<T>
    {
        T Clone();
    }
}

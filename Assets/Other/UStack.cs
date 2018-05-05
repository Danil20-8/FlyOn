using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public class UStack<Tkey, Tvalue> : IEnumerable<UStackKeyValue<Tkey, Tvalue>>
    {
        UStackNode<UStackKeyValue<Tkey, Tvalue>> first = null;
        UStackNode<UStackKeyValue<Tkey, Tvalue>> end = null;
        public bool Push(Tkey element, out UStackKeyValue<Tkey, Tvalue> result)
        {
            if (first == null) {
                first = new UStackNode<UStackKeyValue<Tkey, Tvalue>>(new UStackKeyValue<Tkey, Tvalue>() { key = element});
                end = first;

                result = end.item;
                return true;
            }
            var e = BackFind(element);
            if (e != null)
            {
                if (e != end)
                {
                    if (e == first)
                        first = e.next;
                    else
                        e.previous.next = e.next;
                    end.next = e;
                    end = e;
                }
                result = end.item;
                return false;
            }
            else {
                end.next = new UStackNode<UStackKeyValue<Tkey, Tvalue>>(new UStackKeyValue<Tkey, Tvalue>() { key = element });
                end = end.next;

                result = end.item;
                return true;
            }
        }
        UStackNode<UStackKeyValue<Tkey, Tvalue>> BackFind(Tkey item)
        {
            var last = end;
            while (last != null)
            {
                if (last.item.key.Equals(item))
                    return last;
                last = last.previous;
            } 
            return null;
        }
        public void RemoveAll(Predicate<Tvalue> predicate)
        {
            var last = first;
            while (last != null)
            {
                if (predicate(last.item.value))
                    Remove(last);
                last = last.next;
            } 
        }
        public int RemoveWhile(Predicate<Tvalue> predicate)
        {
            int i = 0;
            while (first != null)
            {
                if (!predicate(first.item.value))
                    goto exit;
                i++;
                first = first.next;
            }
            end = first;

            exit:
            return i;
        }
        void Remove(UStackNode<UStackKeyValue<Tkey, Tvalue>> element)
        {
            if (element == end) {
                if (element == first)
                    first = end = null;
                else
                    end = element.previous;
            }
            else if (element == first)
                first = element.next;
            else
                element.previous.next = element.next;
        }

        public IEnumerator<UStackKeyValue<Tkey, Tvalue>> GetEnumerator()
        {
            var last = end;
            while (last != null)
            {
                yield return last.item;
                last = last.previous;
            } 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class UStackNode<T>
    {
        public T item;
        public UStackNode<T> next = null;
        public UStackNode<T> previous = null;
        public UStackNode(T item)
        {
            this.item = item;
        }
    }
    public class UStackKeyValue<Tkey, Tvalue>
    {
        public Tkey key;
        public Tvalue value;
    }
}

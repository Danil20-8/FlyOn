using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    [Serializable]
    public struct LineBounds
    {
        public float left;
        public float right;

        public LineBounds(float left, float right)
        {
            this.left = left;
            this.right = right;
        }

        public bool InBounds(float value)
        {
            return value >= left && value <= right;
        }

        public void ToBounds(ref float value)
        {
            if (value < left) value = left;
            else if (value > right) value = right;
        }
    }
    [Serializable]
    public struct LineBounds<T> where T : IComparable
    {
        public T left;
        public T right;

        public LineBounds(T left, T right)
        {
            this.left = left;
            this.right = right;
        }

        public bool InBounds(T value)
        {
            return value.CompareTo(left) >= 0 && value.CompareTo(right) <= 0;
        }

        public void ToBounds(ref T value)
        {
            if (value.CompareTo(left) < 0) value = left;
            else if (value.CompareTo(right) > 0) value = right;
        }

        public static bool InBounds(T value, T left, T right)
        {
            return value.CompareTo(left) >= 0 && value.CompareTo(right) <= 0;
        }
        
    }

    public struct SquareBounds
    {
        public float leftX, rightX;
        public float leftY, rightY;

        public SquareBounds(float leftX, float rightX, float leftY, float rightY)
        {
            this.leftX = leftX;
            this.rightX = rightX;

            this.leftY = leftY;
            this.rightY = rightY;
        }

        public bool InBoubds(float x, float y)
        {
            return x >= leftX && x <= rightX && y >= leftY && y <= rightY;
        }

        public bool ToBounds(ref UnityEngine.Vector3 vector)
        {
            return ToBounds(ref vector.x, ref vector.y);
        }

        public bool ToBounds(ref UnityEngine.Vector2 vector)
        {
            return ToBounds(ref vector.x, ref vector.y);
        }

        public bool ToBounds(ref float x, ref float y)
        {
            bool moved = false;
            if (x < leftX)
            {
                x = leftX;
                moved = true;
            }
            else if (x > rightX)
            {
                x = rightX;
                moved = true;
            }
            if (y < leftY)
            {
                y = leftY;
                moved = true;
            }
            else if (y > rightY)
            {
                y = rightY;
                moved = true;
            }
            return moved;
        }
    }

    public struct SquareBounds<T> where T : IComparable
    {
        public T leftX, rightX;
        public T leftY, rightY;

        public SquareBounds(T leftX, T rightX, T leftY, T rightY)
        {
            this.leftX = leftX;
            this.rightX = rightX;

            this.leftY = leftY;
            this.rightY = rightY;
        }

        public bool InBoubds(T x, T y)
        {
            return x.CompareTo(leftX) >= 0 && x.CompareTo(rightX) <= 0 && y.CompareTo(leftY) >= 0 && y.CompareTo(rightY) <= 0;
        }

        public bool ToBounds(ref T x, ref T y)
        {
            bool moved = false;
            if(x.CompareTo(leftX) < 0)
            {
                x = leftX;
                moved = true;
            }
            else if(x.CompareTo(rightX) > 0)
            {
                x = rightX;
                moved = true;
            }
            if(y.CompareTo(leftY) < 0)
            {
                y = leftY;
                moved = true;
            }
            else if(y.CompareTo(rightY) > 0)
            {
                y = rightY;
                moved = true;
            }
            return moved;
        }
    }
}

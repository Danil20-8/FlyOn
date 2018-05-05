using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Modern;
using System.Threading;

namespace Assets.Other
{
    public static class PRun
    {
        static int finished = 0;
        static object thisLock = new object();
        static readonly int ptc = 1; // Environment.ProcessorCount;


        public static void For(int length, Action<int> action)
        {
            int ptc = PRun.ptc - 1;
            float sc = ((float)length) / PRun.ptc;
            int i = 0;
            for (; i < ptc; i++)
            {
                ForAction t;
                t.forAction = action;
                t.left = (int)(sc * i);
                t.right = (int)(sc * (i + 1));
                ThreadPool.QueueUserWorkItem(For, t);
            }
            int left = (int)(sc * i), right = (int)(sc * (i + 1) + .5f);
            for (int j = left; i < right; i++)
                action(j);

            while (finished != ptc) ;
            finished = 0;
        }

        static void For(object args)
        {
            ForAction t = (ForAction)args;

            for (int i = t.left; i < t.right; i++)
            {
                t.forAction(i);
            }

            Interlocked.Increment(ref finished);
        }

        public static void Split(int length, Action<int, int> forAction)
        {
            int ptc = PRun.ptc - 1;
            float sc = ((float)length) / PRun.ptc;

            int i = 0;
            for (; i < ptc; i++)
            {
                ForStruct t3;
                t3.forAction = forAction;
                t3.left = (int)(sc * i);
                t3.right = (int)(sc * (i + 1));
                ThreadPool.QueueUserWorkItem(Split, t3);
            }

            forAction((int)(sc * i), (int)(sc * (i + 1) + .5f));

            while (finished != ptc) ;

            finished = 0;
        }
        static void Split(object args)
        {
            ForStruct t3 = (ForStruct)args;

            t3.forAction(t3.left, t3.right);

            Interlocked.Increment(ref finished);
        }
    }

    struct ForStruct
    {
        public Action<int, int> forAction;
        public int left;
        public int right;
    }

    struct ForAction
    {
        public Action<int> forAction;
        public int left;
        public int right;
    }
}

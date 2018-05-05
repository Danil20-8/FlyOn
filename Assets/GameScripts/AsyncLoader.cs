using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections;

public class AsyncProcessor
{
    public bool allDone() { return processes.All(p => p.IsDone()); }

    List<AsyncReport> processes = new List<AsyncReport>();

    MonoBehaviour loader;
    public AsyncProcessor(MonoBehaviour loader)
    {
        this.loader = loader;
    }

    public void Process(Func<IEnumerator> process)
    {
        var p = new AsyncReport(process);
        processes.Add(p);
        loader.StartCoroutine(p);
    }

    class AsyncReport : IEnumerator
    {
        bool done;
        IEnumerator process;


        public object Current
        {
            get
            {
                return process.Current;
            }
        }

        public bool IsDone() { return done; }

        public bool MoveNext()
        {
            if (process.MoveNext())
                return true;
            else
            {
                done = true;
                return false;
            }
        }

        public void Reset()
        {
            process.Reset();
        }

        public AsyncReport(Func<IEnumerator> process)
        {
            this.process = process();
        }
    }
}

public interface IAsyncLoader
{
    void Initialize(AsyncProcessor asyncProcessor);
}

public class AsyncProcess
{
    bool processing;
    bool done;
    Action callback;
    Action action;

    public AsyncProcess(Action action, Action callback = null)
    {
        this.callback = callback;
        this.action = action;
    }
    public WaitUntil Process()
    {
        if (!processing)
        {
            processing = true;
            ThreadPool.QueueUserWorkItem(Run);
            return new WaitUntil(IsDone);
        }
        else
            throw new Exception("AsyncProcess already started");
    }
    void Run(object arg)
    {
        action();
        done = true;
        if (callback != null)
            callback();
    }
    public bool IsDone()
    {
        return done;
    }
}
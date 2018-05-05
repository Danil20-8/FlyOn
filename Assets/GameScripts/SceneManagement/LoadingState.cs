using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LoadingState
{
    public bool cancel;

    Action<string, float> callback;

    public LoadingState(Action<string, float> callback)
    {
        this.callback = callback;
    }

    public void Callback(string processName, float progress)
    {
        callback(processName, progress);
    }
}


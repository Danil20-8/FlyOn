using UnityEngine;
using System.Collections;
using System;

public class FlipScreenManager : ScreenManager {
    [SerializeField]
    new Camera camera;

    protected override Func<bool> Transition(SlideBehaviour from, SlideBehaviour to)
    {
        return () =>
        {
            camera.transform.position = to.transform.position;
            return true;
        };
    }
}

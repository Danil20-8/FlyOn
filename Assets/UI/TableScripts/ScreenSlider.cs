using UnityEngine;
using System.Collections;
using System;

public class ScreenSlider : ScreenManager {

    [SerializeField]
    new Camera camera;

    float speed = 1f;
    float timeStart;

    SlideBehaviour from;
    SlideBehaviour to;

    protected override void OnStart (SlideBehaviour beginSlide) {
        camera.transform.position = beginSlide.transform.position;
	}
	
	// Update is called once per frame
	bool Transit () {

        float rate = 1f / speed * (Time.time - timeStart);
        if (rate < 1)
        {
            camera.transform.position = from.transform.position + (to.transform.position - from.transform.position) * rate;
            return false;
        }
        else
        {
            camera.transform.position = to.transform.position;
            return true;
        }
	}
    protected override Func<bool> Transition(SlideBehaviour from, SlideBehaviour to)
    {
        this.from = from;
        this.to = to;
        return Transit;
    }
}


using UnityEngine;
using System.Collections;

public abstract class SlideBehaviour : MonoBehaviour {

    protected ScreenManager screenSlider;
    public string label;
    public Transform messageTransform;
    public void SetSlider(ScreenManager slider)
    {
        screenSlider = slider;
    }

    public virtual void OnSlide(params object[] args)
    {

    }
    public virtual void OnOut()
    {

    }

}

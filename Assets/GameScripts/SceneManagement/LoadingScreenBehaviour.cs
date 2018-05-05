using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;


public class LoadingScreenBehaviour : MonoBehaviour
{
    [SerializeField]
    new Camera camera;

    public LoadingState state { get; private set; }

    protected void Awake()
    {
        state = new LoadingState(UpdateView);
    }

    protected virtual void UpdateView(string processName, float progress)
    {

    }

    public void Begin()
    {
        GameManager.instance.SetCamera(camera.gameObject);
        OnBegin();
    }
    protected virtual void OnBegin()
    {

    }
    public void End()
    {
        GameManager.instance.ReleaseCamera(camera.gameObject);
        OnEnd();
    }
    protected virtual void OnEnd()
    {

    }
}


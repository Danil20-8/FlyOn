using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
    [SerializeField]
    new GameObject camera;

    void Awake()
    {
        GameManager.instance.SetCamera(camera);
    }

    void OnDestroy()
    {
        GameManager.instance.ReleaseCamera(camera);
    }
}



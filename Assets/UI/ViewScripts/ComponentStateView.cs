using UnityEngine;
using System.Collections;
using System;
using Assets.GameModels;
public class ComponentStateView : MonoBehaviour {

    [SerializeField]
    TextsView view;

    public void SetData(SystemLink.Info info)
    {
        view.SetName(info.name);
        view.SetAlive(info.alive);
        view.SetEnabled(info.enabled);
    }
}

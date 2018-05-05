using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ThemeListener: MonoBehaviour
{
    void Start()
    {
        ThemeManagerBehaviour.AddListener(this);
    }

    public void UpdateTheme()
    {
        Color c = ThemeManagerBehaviour.GetColor(ThemeElementType.Normal);
        foreach (var i in GetComponentsInChildren<Image>(true))
        {
            var preserver = i.GetComponent<ThemePreserver>();
            if (preserver == null)
            {
                i.color = c;
            }
        }

        c = ThemeManagerBehaviour.GetColor(ThemeElementType.Text);
        foreach (var t in GetComponentsInChildren<Text>(true))
        {
            var preserver = t.GetComponent<ThemePreserver>();
            if (preserver == null)
                t.color = c;
        }

        foreach(var s in GetComponentsInChildren<Selectable>(true))
        {
            var preserver = s.GetComponent<ThemePreserver>();
            if (s.transition != Selectable.Transition.Animation && preserver == null)
            {
                s.transition = Selectable.Transition.Animation;
                s.gameObject.AddComponent<Animator>().runtimeAnimatorController = ThemeManagerBehaviour.currentManager.controller;
            }
        }

    }

    void OnEnable()
    {
        if(ThemeManagerBehaviour.isAble)
            UpdateTheme();
    }

    void OnDestroy()
    {
        ThemeManagerBehaviour.RemoveListener(this);
    }
}

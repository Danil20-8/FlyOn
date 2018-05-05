using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Toggle))]
public class BoolInput : MonoBehaviour, IAutoInput {

    Toggle toggle;

    public void Init(ValueOption value)
    {
        toggle = GetComponent<Toggle>();

        toggle.isOn = (bool)value.value;

        toggle.onValueChanged.AddListener((v) => value.setAction(v));
    }

    public bool IsAble(Type type)
    {
        return type == typeof(bool);
    }
}

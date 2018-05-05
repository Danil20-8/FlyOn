using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class StringInputContainerView : ContainerItemView {

    [SerializeField]
    InputField input;

    public event System.Action<string> onValueChanged;

    bool internalChange;

    void Awake()
    {
        input.onValueChanged.AddListener(OnValueChanged);
    }

    protected override void OnSetModel(ref object model)
    {
        string str = (string)model;

        internalChange = true;

        input.text = str;
    }

    void OnValueChanged(string str)
    {
        if(internalChange)
        {
            internalChange = false;
            return;
        }
        SetModel(str);

        if (onValueChanged != null)
            onValueChanged.Invoke(str);
    }
}

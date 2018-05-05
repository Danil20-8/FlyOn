using UnityEngine;
using System.Collections;
using System;
using Assets.Other;
public class StringArrayInputView : ContainerItemView, IAutoInput
{
    [SerializeField]
    ContainerListView list;

    protected override void OnSetModel(ref object model)
    {
        string[] arr = (string[])model;
        list.Clear();
        list.AddRange((string[])model);
        for (int i = 0; i < arr.Length; i++)
        {
            int index = i;
            ((StringInputContainerView)list.GetItem(i)).onValueChanged += s => arr[index] = s;
        }
        UpdateValue((string[])model);
    }

    public void AddElement(string value)
    {
        string[] arr = GetModel<string[]>();
        Algs.IncreseArray(ref arr, arr.Length + 1);
        arr[arr.Length - 1] = value;
        SetModel(arr);
    }
    public void AddElement()
    {
        AddElement("Resource" + GetModel<string[]>().Length);
    }


    ValueOption value;
    void UpdateValue(string[] arr)
    {
        if (value != null)
            value.setAction(arr);
    }

    public void Init(ValueOption value)
    {
        this.value = value;
        SetModel(value.value);
    }

    public bool IsAble(Type type)
    {
        return type == typeof(string[]);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Other;

[RequireComponent(typeof(Slider))]
public class InputRangeView : MonoBehaviour, IAutoInput
{
    Slider slider;

    [SerializeField]
    Text text;

    ValueOption valueOption;

    public void Init(ValueOption value)
    {
        slider = GetComponent<Slider>();
        valueOption = value;
        if (value.value is RangeValue<float>)
        {
            RangeValue<float> rv = (RangeValue<float>)value.value;
            slider.minValue = rv.minValue;
            slider.maxValue = rv.maxValue;

            slider.onValueChanged.AddListener(SetFloat);
            slider.value = rv.value;
        }
        else
        {
            RangeValue<int> rv = (RangeValue<int>)value.value;
            slider.minValue = rv.minValue;
            slider.maxValue = rv.maxValue;

            slider.onValueChanged.AddListener(SetInt);
            slider.value = rv.value;
        }
    }

    public bool IsAble(Type type)
    {
        return type == typeof(RangeValue<float>) || type == typeof(RangeValue<int>);
    }

    void SetFloat(float value)
    {
        text.text = value.ToString();
        valueOption.setAction(new RangeValue<float>(slider.minValue, slider.maxValue, slider.value));
    }
    void SetInt(float value)
    {
        text.text = ((int)value).ToString();
        valueOption.setAction(new RangeValue<int>((int)slider.minValue, (int)slider.maxValue, (int)slider.value));
    }
}


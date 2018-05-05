using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Assets.Other;
using UnityEngine.UI;
using Assets.Global;

public partial class ValueChangeOption : ContainerItemView {

    [SerializeField]
    InputField input;

    [SerializeField]
    Dropdown _dropdownInput;

    [SerializeField]
    MonoBehaviour[] inputs;

    ValueOption value;

    protected override void OnSetModel(ref object model)
    {
        value = (ValueOption) model;

        SetField();
    }

    void SetField()
    {
        var val = value.value;

        Type type = val.GetType();
        foreach (var i in inputs)
        {
            var autoInput = (IAutoInput)i;
            if (autoInput.IsAble(type))
            {
                ((IAutoInput)Instantiate(i, transform.position, transform.rotation, transform))
                    .Init(value);
                return;
            }
        }

        if (val is int)
            SetTextInputFieldInt();
        else if (val is float)
            SetTextInputFieldFloat();
        else if (val is Enum)
            SetDropdownFieldEnum();
        else
            throw new Exception("Bad value");
    }

    void SetTextInputFieldFloat()
    {
        InputField input = (InputField)Instantiate(this.input, Vector3.zero, Quaternion.identity, transform);
        input.transform.localPosition = Vector3.zero;

        input.onEndEdit.AddListener(s => value.setAction(float.Parse(s)));

        input.contentType = InputField.ContentType.DecimalNumber;

        input.text = ((float)value.value).ToString();
    }
    void SetTextInputFieldInt()
    {
        InputField input = (InputField)Instantiate(this.input, Vector3.zero, Quaternion.identity, transform);
        input.transform.localPosition = Vector3.zero;

        input.onEndEdit.AddListener(s => value.setAction(int.Parse(s)));

        input.contentType = InputField.ContentType.IntegerNumber;

        input.text = ((int)value.value).ToString();
    }
    void SetDropdownFieldEnum()
    {
        Dropdown input = (Dropdown)Instantiate(_dropdownInput, Vector3.zero, Quaternion.identity, transform);
        input.transform.localPosition = Vector3.zero;

        input.onValueChanged.AddListener(n => value.setAction(n));

        input.options = new List<Dropdown.OptionData>(Enum.GetNames(value.value.GetType()).Select(n => new Dropdown.OptionData(Localization.GetGlobalString(n))));

        input.value = (int) value.value;
    }
}

public class ValueOption
{
    public string optionName;

    public OptionType type;

    public object value;

    public Action<object> setAction;
}

public enum OptionType
{
    FLoatValue,
    IntValue,
    StringValue

}

public interface IAutoInput
{
    bool IsAble(Type type);

    void Init(ValueOption value);

}
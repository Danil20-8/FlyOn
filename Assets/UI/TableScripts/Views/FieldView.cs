using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FieldView : ContainerItemView {

    [SerializeField]
    Text nameText;
    [SerializeField]
    Text valueText;

    public string fieldName { get { return nameText.text; } set { nameText.text = value; } }
    public string fieldValue { get { return valueText.text; } set { valueText.text = value; } }

    public void SetField(string name, string value)
    {
        fieldName = name;
        fieldValue = value;
    }
}

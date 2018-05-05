using UnityEngine;
using System.Collections;

public class TextInputField : MonoBehaviour {

    [SerializeField]
    TextMesh textField;

    public string text { get { return textField.text; } set { textField.text = value; } }
	
	// Update is called once per frame
	void Update () {

        if(text.Length > 0)
            if (Input.GetKeyDown(KeyCode.Backspace))
                text = text.Remove(text.Length - 1);

        var s = Input.inputString;
        if (s == "")
            return;
        text += s;
	}
}

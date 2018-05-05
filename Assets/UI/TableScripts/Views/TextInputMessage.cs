using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class TextInputMessage : MonoBehaviour {

    [SerializeField]
    Text messageText;
    [SerializeField]
    Text attentionMessage;
    [SerializeField]
    InputField text;

    Func<string, string> action;

    public void SetMessageBox(string messageText, Func<string, string> action)
    {
        if (this.action != null)
            throw new Exception("Action's already seted");
        this.action = action;
        this.messageText.text = messageText;
    }

    public void OK()
    {
        var answer = action(text.text);
        if(answer == "")
            Destroy(gameObject);

        attentionMessage.text = answer;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class PressKeyMessage : MonoBehaviour
{
    [SerializeField]
    Text messageText;
    [SerializeField]
    Text errorMessageText;
    Func<string, string> onPress;

    public void SetMessageBox(string message, Func<string, string> onPress)
    {
        messageText.text = message;
        this.onPress = onPress;
    }
    void Update()
    {
        for(char c = 'a'; c <= 'z'; c++)
            if(Input.GetKeyDown(new string(c, 1)))
            {
                var r = onPress(new string(new char[] { c }));
                if (r == "")
                    Destroy(gameObject);
                else
                    errorMessageText.text = r;
            }
    }

}


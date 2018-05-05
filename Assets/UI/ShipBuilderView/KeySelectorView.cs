using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Assets.GameModels;
using UnityEngine.UI;
public class KeySelectorView : MonoBehaviour
{

    bool listening = false;
    [SerializeField]
    Text componentName;
    [SerializeField]
    Text componentKey;
    [SerializeField]
    PressKeyMessage messageAsset;
    Transform message;

    SystemComponentView current;
    // Use this for initialization
    void Start()
    {
        GetComponentInParent<PointerListenerBehaviour>()
            .AddClickListener<SystemComponentView>(b => UpdateView((SystemComponentView)b));
    }

    void UpdateView(SystemComponentView component)
    {
        current = component;
        componentName.text = current.GetItem().lName;
        componentKey.text = current.key.ToUpper();
    }
    public void Listen()
    {
        if (listening || current == null)
            return;
        listening = true;
        ScreenManager.ShowMessage(messageAsset).SetMessageBox("sssss", Listen);
    }
    string Listen(string s)
    {
        if(s[0] >= 'a' && s[0] <= 'z' || s[0] >= 'Z' && s[0] <= 'Z')
        componentKey.text = s.ToUpper();
        current.key = s;
        listening = false;
        return "";
    }
}

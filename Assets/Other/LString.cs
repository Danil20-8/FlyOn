using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;
using Assets.Serialization;
using MyLib.Algoriphms;
using MyLib.Serialization;
using MyLib.Serialization.Binders;
using Assets.Other;
[Serializable]
public class LString
{
    [SerializeField]
    string stringName;
    string result;
    public string lText { get { if (result == null) result = Get(); return result; } }
    public string text { get { return stringName; } set { if (stringName != value) { result = null; stringName = value; } } }
    public LString(string text)
    {
        this.stringName = text;
    }
    public LString()
    {
        this.stringName = "NotFilled";
    }

    string Get()
    {
        return Localization.instance.GetString(stringName);
    }
    public static explicit operator LString(string str)
    {
        return new LString(str);
    }
    public static implicit operator string(LString lstr)
    {
        return lstr.lText;
    }
    public static bool operator != (LString lstr, string str)
    {
        return lstr.stringName != str;
    }
    public static bool operator ==(LString lstr, string str)
    {
        return lstr.stringName == str;
    }

}

public enum Language
{
    Russian,
    English
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ThemeManagerBehaviour: MonoBehaviour
{
    public static ThemeManagerBehaviour currentManager { get; private set; }
    public static bool isAble { get; private set; }
    [SerializeField]
    ThemeData[] data;

    public RuntimeAnimatorController controller;

    List<ThemeListener> listeners = new List<ThemeListener>();

    ThemeData current;
    int _curr = 0;

    void Awake()
    {
        current = data[_curr];

        currentManager = this;
        isAble = true;
    }

    public void NextTheme()
    {
        _curr++;
        if (_curr == data.Length)
            _curr = 0;

        current = data[_curr];
        foreach (var l in listeners)
            l.UpdateTheme();
    }

    public static Color GetColor(ThemeElementType type)
    {
        return currentManager.current[type];
    }



    public static void AddListener(ThemeListener listener)
    {
        currentManager.listeners.Add(listener);
        listener.UpdateTheme();
    }
    public static void RemoveListener(ThemeListener listener)
    {
        currentManager.listeners.Remove(listener);
    }
}

[Serializable]
public class ThemeData
{
    public Color normal;
    public Color highlighted;
    public Color pressed;
    public Color disabled;
    public Color text;

    public Color this[ThemeElementType type]
    {
        get
        {
            switch(type)
            {
                case ThemeElementType.Normal:
                    return normal;
                case ThemeElementType.Highlighted:
                    return highlighted;
                case ThemeElementType.Pressed:
                    return pressed;
                case ThemeElementType.Disabled:
                    return disabled;
                case ThemeElementType.Text:
                    return text;
                default:
                    return Color.white;
            }
        }
    }
}

public enum ThemeElementType
{
    Normal,
    Highlighted,
    Pressed,
    Disabled,
    Text
}
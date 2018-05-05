using System;
using UnityEngine;
using System.Linq;

interface UniversalInput
{
    float Horizontal();
    float Vertical();
    float Fire();
    float Fire2();
    float RotateX();
    float RotateY();
    bool key1();
    bool key2();

    float scroll();
}

static class UInput
{
    public static UniversalInput input = new MKInput();

    public static float Horizontal()
    {
        return input.Horizontal();
    }
    public static float Vertical()
    {
        return input.Vertical();
    }
    public static float Fire()
    {
        return input.Fire();
    }
    public static float Fire2()
    {
        return input.Fire2();
    }
    public static bool key1()
    {
        return input.key1();
    }
    public static bool key2()
    {
        return input.key2();
    }
    public static float RotateX()
    {
        return input.RotateX();
    }
    public static float RotateY()
    {
        return input.RotateY();
    }
}

class MKInput :
    UniversalInput
{

    string[] keys;

    public MKInput()
    {
        Type t = typeof(KeyCode);
        keys = t.GetFields().Select(f => f.Name).Where(s => s.Length < 2).Select(s => s.ToLower()).ToArray();
    }

    public bool key1()
    {
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    public bool key2()
    {
        return Input.GetKeyDown(KeyCode.Alpha2);
    }

    public float RotateX()
    {
        return Input.GetAxis("Mouse X");
    }

    public float RotateY()
    {
        return Input.GetAxis("Mouse Y");
    }

    public float scroll()
    {
        return Input.mouseScrollDelta.y;
    }

    float UniversalInput.Fire()
    {
        return Input.GetAxis("Fire1");
    }
    float UniversalInput.Fire2()
    {
        return Input.GetAxis("Fire2");
    }
    float UniversalInput.Horizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    float UniversalInput.Vertical()
    {
        return Input.GetAxis("Vertical");
    }
}


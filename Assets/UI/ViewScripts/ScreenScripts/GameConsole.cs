using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameConsole: MonoBehaviour
{
    [SerializeField]
    Text answer;

    [SerializeField]
    InputField commandLine;

    Func<string, string[], string> func;

    void Start()
    {
        commandLine.Select();
    }

    public void WaitingForCommand(Func<string, string[], string> func)
    {
        this.func = func;
    }

    void Update()
    {
        if (func != null)
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var line = commandLine.text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length == 1)
                {
                    answer.text = func(line[0], new string[0]);
                }
                if (line.Length > 1)
                {
                    answer.text = func(line[0], line.Skip(1).ToArray());
                }
                commandLine.text = "";
            }
    }
}

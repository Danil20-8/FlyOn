using UnityEngine;
using System.Collections;

public class MainMenuViewBehaviour : SlideBehaviour {

    void Start()
    {
        GameManager.instance.PinCurrent();
    }

    public void QuickGame()
    {
        screenSlider.MoveTo("BattleConfig");
    }

    public void MoveToEditor()
    {
        screenSlider.MoveTo("selector");
    }

    public void MoveToSettings()
    {
        screenSlider.MoveTo("Settings");
    }

    public void Exit()
    {
        Application.Quit();
    }

    [GameConsoleCommand]
    string Hello(string name)
    {
        return name + ": Hello you too!";
    }
}

using UnityEngine;
using System.Collections;
using Assets.Global;

public class BattleMenuBehaviour : SlideBehaviour {

	public void MoveToMainMenu()
    {
        BattleBehaviour.current.EndBattle()
            .Invoke();
    }

    public override void OnSlide(params object[] args)
    {
        BattleBehaviour.current.paused = true;
        Cursor.visible = true;
    }

    public void Continue()
    {
        screenSlider.MoveTo("PlayerView");
    }
}

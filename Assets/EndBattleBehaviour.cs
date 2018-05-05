using System;
using UnityEngine;
using System.Collections;
using Assets.Global;

public class EndBattleBehaviour : MonoBehaviour {

    new Transform camera;
    Transform sun;

    float _speed;

    Action endAction;
	void Awake () {

        enabled = false;
	}
	
	public void StartEndBattle(Transform camera, Transform sun, Action endAction)
    {
        Cursor.visible = true;

        this.camera = camera;
        this.sun = sun;
        this.endAction = endAction;

        _speed = 24f;

        this.camera.forward = (this.sun.position - this.camera.position).normalized;

        FindObjectOfType<WatcherBehaviour>().target = null;

        var battleInfo = BattleBehaviour.current.GetInfo();

        ScreenManager.currentManager.MoveTo("EndBattleView");

        enabled = true;
    }

    void Update()
    {
        camera.RotateAround(sun.position, camera.up, _speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {   
            endAction();
        }
    }
}

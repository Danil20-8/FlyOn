using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Global;
public class DeadViewBehaviour : SlideBehaviour {

    [SerializeField]
    Text resurrectText;

    string resurrectIn;

    float nextRespawn;
    float lastRefresh;

	void Start () {
        resurrectIn = Localization.GetGlobalString("ResurrectIn") + " ";
	}

    public override void OnSlide(params object[] args)
    {
        nextRespawn = Mathf.Round(BattleBehaviour.current.GetUpdater<RespawnBehaviour>().nextRespawn);
        Cursor.visible = false;
    }

    void Refresh()
    {
        lastRefresh = Mathf.Round(Time.time);
        resurrectText.text = resurrectIn + (nextRespawn - lastRefresh);
    }

    void Update () {
        if (Time.time > nextRespawn)
        {
            screenSlider.MoveTo("PlayerView");
        }
         else if (Time.time - lastRefresh >= 1f)
        {
            Refresh();
        }
	}
}

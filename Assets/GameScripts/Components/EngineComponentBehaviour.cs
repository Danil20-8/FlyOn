using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.GameModels;
using UnityEngine;


public class EngineComponentBehaviour: ShipComponentBehaviour
{

    [SerializeField]
    GameObject activeView;

    bool changed = false;
    bool active = false;

    int type;

    protected override void OnSetLink(SystemComponent component)
    {
        type = component.shipClass;
    }

    public void Enable()
    {
        active = true;
        changed = true;
    }
    public void Disable()
    {
        active = false;
        changed = true;
    }

    protected override void SlowUpdate()
    {
        if (driver is PlayerDriver)
            if (changed)
            {
                switch (active)
                {
                    case true:
                        BattleBehaviour.current.audioPlayer.PlayLoop("Engine" + type, transform.position);
                        activeView.SetActive(true);
                        break;
                    case false:
                        BattleBehaviour.current.audioPlayer.StopLoop("Engine" + type);
                        activeView.SetActive(false);
                        break;
                }

                changed = false;
            }
    }

}


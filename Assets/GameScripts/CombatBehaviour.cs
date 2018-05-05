using UnityEngine;
using System.Collections;
using System;
public class CombatBehaviour : MonoBehaviour {

    Action<float, ShipController> onDamage;
    Action onDestroy;

    public void SetOnDamage(Action<float, ShipController> onDamage)
    {
        this.onDamage = onDamage;
    }
    public void SetDamage(float damage, ShipController offender = null)
    {
        onDamage(damage, offender);
    }
    public void SetOnDestroy(Action onDestroy)
    {
        this.onDestroy = onDestroy;
    }

    bool isExist = true;
    public void Destroy()
    {
        if (isExist)
        {
            onDestroy();
            isExist = false;
        }
    }
    public static void SetColor(float health, params Renderer[] items)
    {
        Color c = new Color(.8f + .2f * (1 - health), .8f * health, .8f * health);
        foreach (var r in items)
            r.material.color = c;
    }
}

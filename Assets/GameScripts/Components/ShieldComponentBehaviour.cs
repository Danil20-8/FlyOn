using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.GameModels;
using UnityEngine;

public class ShieldComponentBehaviour : ShipComponentBehaviour
{
    [SerializeField]
    ShieldPlane shield;

    float capacity;
    public void UpdateShield(float capacity)
    {
        this.capacity = capacity;
        if (capacity > 0 && !shield.gameObject.activeSelf)
            shield.gameObject.SetActive(true);
    }
    public void Disable()
    {
        shield.gameObject.SetActive(false);
    }
    public bool IsBreached(float damage)
    {
        return damage >= capacity;
    }
}


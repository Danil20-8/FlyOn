using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AimBehaviour : MonoBehaviour
{
    [SerializeField]
    Text distanceText;

    [SerializeField]
    Material enemy;
    [SerializeField]
    Material friend;
    [SerializeField]
    Material neutral;

    Image aim;
    Material none;
    void Start()
    {
        aim = GetComponent<Image>();
        none = aim.material;
    }
    public void SetTarget(AimTarget target, float distance)
    {
        switch (target)
        {
            case AimTarget.None:
                SetColor(none);
                break;
            case AimTarget.Enemy:
                SetColor(enemy);
                break;
            case AimTarget.Friend:
                SetColor(friend);
                break;
            case AimTarget.Neutral:
                SetColor(neutral);
                break;
        }
        if (target != AimTarget.None)
            distanceText.text = distance.ToString();
        else
            distanceText.text = "";
    }
    void SetColor(Material material)
    {
        aim.material = material;
    }

}

public enum AimTarget
{
    None,
    Enemy,
    Friend,
    Neutral
}


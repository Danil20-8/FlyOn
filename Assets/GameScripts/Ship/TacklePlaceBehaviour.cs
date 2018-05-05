using UnityEngine;
using System.Collections;

public class TacklePlaceBehaviour : MonoBehaviour {

    public float clipAngle;
    public Quaternion clipRotation;

    public void Init(TacklePlaceBehaviour tackle)
    {
        if (tackle.transform.childCount > 0)
            clipRotation = tackle.transform.localRotation * tackle.transform.GetChild(0).localRotation;
        else
            clipRotation = tackle.clipRotation;
        clipAngle = tackle.clipAngle;
        Destroy(tackle);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Events;
public class Clickable : MonoBehaviour {

    [SerializeField]
    UnityEvent clickEvent;

    void OnMouseDown()
    {
        if (clickEvent != null)
            clickEvent.Invoke();
    }

}

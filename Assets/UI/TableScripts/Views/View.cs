using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MyLib;
using Assets.Other;
public class View : MonoBehaviour {

    void OnDestroy()
    {
        var p = GetComponentInParent<View>();
        transform.SetParent(null);
        if (p != null)
            p.Refresh();
    }
    public virtual void Refresh()
    {

    }
}

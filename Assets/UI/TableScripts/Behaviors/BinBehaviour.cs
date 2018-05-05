using UnityEngine;
using System.Collections;

public class BinBehaviour: MonoBehaviour {

    public virtual bool Put(ContainerItemView item)
    {
        return false;
    }
}

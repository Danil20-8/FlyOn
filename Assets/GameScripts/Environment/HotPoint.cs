using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class HotPoint : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        var s = col.transform.GetComponentInParent<CombatBehaviour>();
        if (s == null)
            return;
        s.SetDamage(float.PositiveInfinity);
        var e = Explosion.Detonate(transform.position, Quaternion.Inverse(transform.rotation), "Big");
        Vector3 v = (s.transform.position - transform.position).normalized;
        e.transform.position = col.transform.position;
        e.transform.localRotation = Quaternion.LookRotation(v);
        s.Destroy();
    }

}

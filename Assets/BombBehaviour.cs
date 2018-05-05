using UnityEngine;
using System.Collections.Generic;
using Assets.Global;
using Assets.Other;
using MyLib.Algoriphms;
public class BombBehaviour : MonoBehaviour {

    [SerializeField]
    int bombClass;

    float speed = 2;
    float damage = 1;
    float radius = 2f;
    bool active = true;

    List<ShipController> shipsAround = new List<ShipController>();

    void Start()
    {
        var cb = gameObject.AddComponent<CombatBehaviour>();
        cb.SetOnDamage(OnDamage);
        speed *= bombClass + 1;
        damage *= bombClass + 1;
        radius *= bombClass + 1;
    }
    // Update is called once per frame
    void Update() {
        BattleBehaviour.current.area.GetObjects(transform.position, 300, shipsAround);
        if (shipsAround.Count == 0)
            return;
        var near = shipsAround.WithMin(s => (s.transform.position - transform.position).sqrMagnitude);
        transform.position += (near.transform.position - transform.position).normalized * speed * Time.deltaTime;
    }
    void OnDamage(float damage, ShipController offender)
    {
        Boom();
    }
    void OnTriggerEnter(Collider col)
    {
        Boom();
    }
    void Boom()
    {
        if (!active)
            return;
        active = false;
        var ss = Physics.OverlapSphere(transform.position, radius);
        foreach (var s in ss)
        {
            var combat = s.GetComponent<CombatBehaviour>();
            if (combat != null)
                combat.SetDamage(damage);
        }
        Explosion.Detonate(transform.position, Quaternion.identity, "Sphere");
        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;
using System;
using Assets.Global;
[RequireComponent(typeof(ParticleSystem))]
public class Explosion : MonoBehaviour, IPoolObject {

    float timeStart;
    ParticleSystem ps;
    [SerializeField]
    new Light light;
    float lightIntens;
    string explosion;

    bool alive = false;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        lightIntens = light.intensity / 2;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - timeStart > ps.duration)
            Destroy();
        light.intensity = lightIntens + lightIntens * Cycle(Time.time);
	}
    public static Explosion Detonate(Vector3 position, Quaternion rotation, string explosion, Transform parent = null)
    {
        var e = BattleBehaviour.current.pool.Get<Explosion>(@"Prefabs\Explosion" + explosion, position, rotation, parent);
        e.timeStart = Time.time;
        e.explosion = explosion;
        e.alive = true;
        return e;
    }
    void OnDisable() //When ship is waiting for Destroy invoke
    {
        if(alive && BattleBehaviour.isBattleNow) //if disabling by not self
            BattleBehaviour.AddEvent(Destroy); //Because can't change parent while disabling/activating
    }
    public void Destroy()
    {
        alive = false;
        transform.SetParent(null);
        BattleBehaviour.current.pool.Put(@"Prefabs\Explosion" + explosion, this);
    }
    float Cycle(float time)
    {
        var t = (time - timeStart) % 2;
        if (t <= 1f)
            return t;
        else
            return 2f - t;
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModels;
using Assets.GameModels.Components;
using Assets.GameModels.ShipEffects;
using Assets.GameModels.Battle;
using Assets.Global;
using Assets.Other;
using MyLib.Modern;
using MyLib.Algoriphms;
[RequireComponent(typeof(Rigidbody))]
public sealed class ShipController : MyMonoBehaviour, ISpatialPartionable {

    public Vector3 position { get { return tempTransform.position; } }

    public string shipName { get { return shipSystem.name; } }
    Rigidbody rigidBody;
    public TempTransform tempTransform;
    public ShipTeam team { get { return shipDriver.shipTeam; } }
    public float health { get; private set; }
    public bool alive { get { return health > 0; } }
    public int shipClass { get; private set; }
    public int mass { get { int mass = shipClass; for (int i = 0; i < shipSystem.power.Length; i++) mass += shipSystem.power[i].item.shipClass; return mass; } }
    public List<Offender> offenders { get; private set; }
    UStack<ShipController, Tuple<float, float>> _offenders = new UStack<ShipController, Tuple<float, float>>();
    float maxHealth = 10;
    public ShipSystem shipSystem { get; private set; }
    ShipDriver shipDriver;
    ShipState state = new ShipState();
    UStack<ShipEffect, float> effects = new UStack<ShipEffect, float>();
    ShipComponentBehaviour[] components;
    public bool isPlayer { get { return shipDriver is PlayerDriver; } }

    void Awake()
    {
        tempTransform = AddMyComponent<TempTransform>();
        rigidBody = GetComponent<Rigidbody>();
        //rigidBody.isKinematic = true;
        //rigidBody.freezeRotation = true;
        rigidBody.useGravity = false;

        offenders = new List<Offender>();
        // shipDriver turns it true
        enabled = false;
    }

    void Start()
    {
        tempTransform.enabled = false;

        var cb = gameObject.AddComponent<CombatBehaviour>();
        cb.SetOnDamage(SetDamage);
        cb.SetOnDestroy(Destroy);
    }

    bool notWaitingForDestroy = true;
    void SetDamage(float damage, ShipController offender)
    {
        if (!state.takeDamage)
            return;

        bool wasAlive = alive;

        DamageHull(damage);
        DamageComponents(damage);

        if(offender != null)
            AddOffender(damage, offender);

        if (wasAlive != alive)
        {
            AddToDead(offender);
        }

    }
    void Destroy()
    {
        Destroy(0, 1f);
    }
    public void Destroy(int framesTo, float timeTo)
    {
        if (notWaitingForDestroy)
        {
            notWaitingForDestroy = false;
            BattleBehaviour.AddEvent(() => DestroyImmediate(), framesTo, timeTo);
        }
    }
    void DestroyImmediate()
    {
        //Sending destroy message to all systems
        BattleBehaviour.current.DestroyShip(this, 0, 0);
        //Deataching explosins in OnDisable message
        gameObject.SetActive(false);
        foreach (var c in components)
            c.gameObject.SetActive(false);
        //Final destroying the ship
        BattleBehaviour.AddEvent(() => { Destroy(gameObject); foreach (var c in components) Destroy(c.gameObject); }, 2);

    }
    void DamageComponents(float damage)
    {
        damage /= components.Length;
        foreach (var c in components)
            c.TakeDamage(damage);
    }

    Material material;
    void DamageHull(float damage)
    {
        health -= damage;
        if (health < 0)
            health = 0;

        shipDriver.OnDamaged(damage);

        var t_material = HealthMaterials.GetMaterial(health / maxHealth);
        if(material != t_material)
        {
            material = t_material;
            GetComponent<Renderer>().material = material;
        }
    }
    
    void SetComponentDamage(float damage, ShipController offender, ShipComponentBehaviour component)
    {
        if (!state.takeDamage)
            return;

        bool wasAlive = alive;

        component.TakeDamage(damage);
        DamageHull(damage);

        if (offender != null)
            AddOffender(damage, offender);

        if (wasAlive != alive)
            AddToDead(offender);
    }

    void AddToDead(ShipController killer)
    {
        foreach (var p in shipSystem.power)
            p.enabled = false;

        shipDriver.OnDead();
        rigidBody.freezeRotation = false;
        team.AddToDead(shipDriver, killer);

        if (killer != null)
            killer.shipDriver.OnKill(this);

        BattleBehaviour.current.RemoveShip(this, 1); // 1 for disable all components
    }

    bool _offendersChanged = false;
    public void AddOffender(float damage, ShipController offender)
    {
        if (offender.team == team)
            return;
        UStackKeyValue<ShipController, Tuple<float, float>> u;
        if (_offenders.Push(offender, out u))
        {
            u.value = new Tuple<float, float>(damage, Time.time);
            _offendersChanged = true;
        }
        else
        {
            u.value.Item1 += damage;
            u.value.Item2 = Time.time;
        }
    }
    public void SetShip(int shipClass, ShipSystemModel model, ShipDriver driver)
    {
        GetComponent<Rigidbody>().mass = Mathf.Pow(2, shipClass);
        shipDriver = driver;
        shipSystem = new ShipSystem(model);
        this.shipClass = shipClass;
        maxHealth = new float[] { 1f, 4f, 12f, 36f, 250f * ((BattleConfig) GameState.sharedData["BattleConfig"]).battleDurationMultipier }[shipClass];// Mathf.Pow(maxHealth, shipClass + 1);
        health = maxHealth;

        shipDriver.SetShip(this); //turn enabled true

        gameObject.AddComponent<ShipIdentification>().ship = this;
    }
    public void SetComponents(ShipComponentBehaviour[] components)
    {
        this.components = components;
        foreach (var c in this.components)
        {
            var com = c;
            c.gameObject.AddComponent<CombatBehaviour>()
                .SetOnDamage((damage, offender) => SetComponentDamage(damage, offender, com));
        }

        foreach (var c in components)
        {
            c.localPosition = c.transform.localPosition;
            c.localRotation = c.transform.localRotation;

            foreach (var col in c.GetComponentsInChildren<Collider>())
                col.isTrigger = true;
            c.transform.parent = null;

            c.gameObject.AddComponent<ShipIdentification>().ship = this;
        }
    }

    protected override void InitializeUpdate()
    {
        if (_offenders.RemoveWhile(t => BattleBehaviour.time - t.Item2 > 5f) > 0 || _offendersChanged)
        {
            offenders = _offenders
                .Select(t => new Offender() { ship = t.key, damage = t.value.Item1, lastTime = t.value.Item2 })
                .ToList();

            _offendersChanged = false;
        }
        tempTransform.InitializeUpdate();

        shipDriver.InitializeUpdate();
        for (int i = 0; i < components.Length; i++)
            components[i].__InitializeUpdate();
    }
    protected override void FastUpdate()
    {
        //effects update
        effects.RemoveWhile(t => BattleBehaviour.time - t > 1);
        state.Reset();
        foreach (var e in effects)
            e.key.Apply(state);

        if (state.updateLinks)
            foreach (var p in shipSystem.power)
                p.Pulse();

        for (int i = 0; i < components.Length; i++)
            components[i].__FastUpdate();

        shipDriver.FastUpdate();
    }
    protected override void SlowUpdate()
    {
        foreach (var c in components)
            c.__SlowUpdate();

        if (state.updateDriver)
            shipDriver.SlowUpdate();
    }
    public void LastUpdate()
    {
        rigidBody.velocity *= 1 - shipClass * .05f * BattleBehaviour.deltaTime;

        var ltw = transform.localToWorldMatrix;
        var rotation = transform.rotation;
        foreach (var c in components)
        {
            c.transform.position = ltw.MultiplyPoint3x4(c.localPosition);
            c.transform.rotation = rotation * c.localRotation;
        }
    }
    void OnGUI()
    {
        if(shipDriver is PlayerDriver)
        {
            if(alive)
                GUI.Label(new Rect(0, 0, 100, 64), (rigidBody.velocity.magnitude * Vector3.Dot(rigidBody.velocity.normalized, transform.forward.normalized)).ToString());
        }
    }
    public void Check()
    {
        shipDriver.Check();
    }
    public void AddEffect(ShipEffect effect)
    {
        UStackKeyValue<ShipEffect, float> u;
        if (effects.Push(effect, out u))
            u.value = Time.time;
        else
        {
            u.value = Time.time;
        }
    }

    public void AddForce(Vector3 force)
    {
        rigidBody.AddForce(force, ForceMode.VelocityChange);
    }
    public void Rotate(Quaternion rotation)
    {
        var t = Quaternion.Angle(rotation, tempTransform.rotation) / 60;
        if (t > 1)
            t = 1;
        if (t > 0)
        {
            var velocity = rigidBody.velocity;
            rigidBody.velocity = velocity * (1 - t) + (velocity * t).magnitude * tempTransform.forward;
        }
        transform.rotation = rotation;
        rigidBody.angularVelocity *= .5f;
    }
    const float impulse_to_damage = .01f;
    void OnCollisionEnter(Collision col)
    {
        var ship = col.transform.GetComponent<ShipController>();
        if (ship != null)
        {
            SetDamage(ship.shipClass / shipClass * col.impulse.magnitude * impulse_to_damage, ship);
        }
        else
            SetDamage(col.impulse.magnitude * impulse_to_damage, null);
        Explosion.Detonate(col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal), "Little");
    }
}

public struct Offender
{
    public ShipController ship;
    public float damage;
    public float lastTime;
}
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using MyLib;
using Assets.Other;
using Assets.Global;
using Assets.GameModels;
using Assets.GameModels.ShipDrivers;
using System;

public class Tir : MonoBehaviour {

    [SerializeField]
    AudioPlayer player;

    [SerializeField]
    BlusterMesh bluster;

    [SerializeField]
    Transform targetPosition;

    Dictionary<Transform, float> healths = new Dictionary<Transform, float>();

    IEnumerator<ShipController> ships;
    ShipController ship;

    void Start()
    {
        ships = InitShip();
        Next();
    }

    void Next()
    {
        if (ship != null)
            DestroyImmediate(ship.gameObject);
        ships.MoveNext();
        ship = ships.Current;

        ship.enabled = true;
    }

    bool _rotating = false;
    void Rotate()
    {
        if (Input.GetMouseButtonDown(1))
            _rotating = true;
        else if (Input.GetMouseButtonUp(1))
            _rotating = false;
        if (_rotating)
        {
            ship.transform.eulerAngles =
                ship.transform.eulerAngles + new Vector3(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"));
        }
        else
        {
            transform.eulerAngles = transform.eulerAngles + new Vector3(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"));
        }
    }

    void Update () {

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Next();
        }

        Rotate();
        
        if(Input.GetMouseButtonDown(0))
        {
            player.Play("BlusterShotAudio", transform.position);
            bluster.Fire(ShotType.Bluster, new TestShot(), transform.position, transform.forward, 100, 3, null);
        }

        bluster.UpdateMesh();
	}

    IEnumerator<ShipController> InitShip()
    {
        var ships = GameResources.GetAllShips()
            .GroupBy(s => GameResources.GetShipHull(s.hull.hullName).shipClass)
            .Where(s => s.Key > 0 && s.Key < 4)
            .OrderBy(s => s.Key)
            .Select(s => s.ToArray());

        begin:

        foreach(var model in ships)
        {
            foreach (var s in model)
            {
                var ship = new CustomShipInitializerModel(s, new FakeDriver()).Init(targetPosition.position, targetPosition.rotation);
                yield return ship;
            }
        }
        goto begin;
    }

    void Variable<T>(T var, System.Action<T> action)
    {
        action(var);
    }
    void With<T>(T var, params System.Action<T>[] actions)
    {
        foreach (var a in actions)
            a(var);
    }
}

struct TestShot : IShot
{
    public void Hit(RaycastHit hit)
    {
        Debug.Log("Here");
    }
}
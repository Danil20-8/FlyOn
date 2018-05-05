using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;
using MyLib.Modern;
using Assets.GameScripts.Battle;
using Assets.GameModels.Battle;
using Assets.Other;

public abstract class BattleBehaviour: MonoBehaviour, ISceneInitializer
{

    public static BattleBehaviour current { get; private set; }
    public static bool isBattleNow { get { return current != null; } }

    public static float time { get; private set; }
    public static float lastTime { get; private set; }
    public static float deltaTime { get; private set; }

    public readonly GamePool pool = new GamePool();
    public AudioPlayer audioPlayer { get; private set; }

    IBattleUpdater[] updaters;
    IAddShipAble[] addShipAbles;
    IRemoveShipAble[] removeShipAbles;
    IDestroyShipAble[] destroyShipAbles;

    List<ShipTeam> teams;
    public Vector3 battleCenter = Vector3.zero;
    public BattleArea area;
    public Navigation navigation = new Navigation();
    public ShotSystem shoting;

    BattleEventUpdater eventUpdater = new BattleEventUpdater();

    public bool paused { get { return Time.timeScale == 0; } set { Time.timeScale = value ? 0 : 1; } }

    bool running = false;

    public Action EndBattle()
    {
        running = false;

        return () =>
        {
            gameObject.SetActive(false);
            current = null;
            GameState.sharedData["BattleInfo"] = GetInfo();
            GameManager.instance.GoToPin();
        };
    }

    public void AddShip(ShipController ship, int timeToAction = 0)
    {
        eventUpdater.AddEvent(() =>
        {
            foreach (var u in addShipAbles)
                u.AddShip(ship);

        }, timeToAction);
    }

    public void RemoveShip(ShipController ship, int timeToAction = 0)
    {
        eventUpdater.AddEvent(() =>
        {
            foreach (var u in removeShipAbles)
                u.RemoveShip(ship);

        }, timeToAction);
    }

    public void DestroyShip(ShipController ship, float timeToDestroy, int timeToAction)
    {
        eventUpdater.AddEvent(() =>
        {
            foreach (var u in removeShipAbles)
                u.RemoveShip(ship);

            foreach (var u in destroyShipAbles)
                u.DestroyShip(ship);

        }, timeToAction, timeToDestroy);
    }
    public static void AddEvent(Action action, int updatesToAction = 0, float timeToAction = 0)
    {
        current.eventUpdater.AddEvent(action, updatesToAction, timeToAction);
    }

    IEnumerator ISceneInitializer.Initialize(AsyncProcessor asyncProcessor, AddListener addDoneListener, LoadingState state)
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
        addDoneListener(LoadingDone);

        yield return Initialize(asyncProcessor, state);
    }

    void LoadingDone()
    {
        enabled = true;
        Time.timeScale = 1;
        Time.fixedDeltaTime = .03f;
    }

    protected abstract IEnumerator Initialize(AsyncProcessor asyncProcessor, LoadingState state);


    public void SetBattle(Vector3 battleCenter, IEnumerable<ShipController> ships, IEnumerable<ShipTeam> teams, IEnumerable<IBattleUpdater> updaters)
    {
        this.battleCenter = battleCenter;

        ShipTeam.SetGoals(teams);
        this.teams = teams.ToList();

        area = new BattleArea(448000, 3500, ShipOutOfBoundsAreaEvent); // x7 bit  // new BattleArea(224000, 3500); // x6 bit

        running = true;
        paused = false;

        time = Time.time;

        this.updaters = updaters.ToArray();
        addShipAbles = new IAddShipAble[] { area }.Concat(updaters.Select(u => u as IAddShipAble).Where(u => u != null)).ToArray();
        removeShipAbles = new IRemoveShipAble[] { }.Concat(updaters.Select(u => u as IRemoveShipAble).Where(u => u != null)).ToArray();
        destroyShipAbles = new IDestroyShipAble[] { area }.Concat(updaters.Select(u => u as IDestroyShipAble).Where(u => u != null)).ToArray();

        foreach (var s in ships)
            foreach (var u in addShipAbles)
                u.AddShip(s);

        audioPlayer = (AudioPlayer)Instantiate(Resources.Load<AudioPlayer>(@"Prefabs\AudioPlayer"), transform.position, transform.rotation, transform);
    }

    void ShipOutOfBoundsAreaEvent(ShipController ship)
    {
        ship.Destroy(0, 0);
    }

    protected void Update()
    {
        eventUpdater.Update();

        if (!running)
            return;

        lastTime = time;
        time = Time.time;
        deltaTime = time - lastTime;

        area.UpdateObjects();

        foreach (var u in updaters)
        {
            u.Update();
        }

        shoting.UpdateShots();


        foreach (var team in teams)
        {
            team.CheckGoals();
            team.Update();
        }


    }

    public T GetUpdater<T>()
    {
        foreach (var u in updaters)
            if (u is T)
                return (T)u;

        throw new Exception(typeof(T).ToString() + " is not found");
    }

    public BattleInfo GetInfo()
    {
        return new BattleInfo(teams);
    }

    protected void Awake()
    {
        current = this;
        enabled = false;
    }

}

public interface IAddShipAble
{
    void AddShip(ShipController ship);
}
public interface IRemoveShipAble
{
    void RemoveShip(ShipController ship);
}
public interface IDestroyShipAble
{
    void DestroyShip(ShipController ship);
}
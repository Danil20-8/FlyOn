using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.GameModels;
using Assets.Global;
using MyLib.Serialization;
using System.Linq;
using System.Reflection;
using Assets.Other;
using MyLib.Algoriphms;
using Assets.GameModels.Battle;
using Assets.GameScripts.Battle;
using Assets.GameModels.Battle.Goals;

public class RandomBattleGround : BattleBehaviour
{
    [SerializeField]
    Transform playerCamera;
    [SerializeField]
    TextAsset[] shipAssets;
    [SerializeField]
    Material skyBox;


    //Battle inialization args
    BattleConfig config;
    Vector3 center;
    List<ShipController> ships = new List<ShipController>();

    ShipTeam[] teams;
    List<IMyMonoBehaviour> otherObjects = new List<IMyMonoBehaviour>();
    new Navigation navigation = new Navigation();

    //Temp items
    float distance;

    SunSystem sunSystem;
    SunBehaviour sun;

    List<IBattleUpdater> _updaters = new List<IBattleUpdater>();

    AsyncProcessor AP;
    protected override IEnumerator Initialize(AsyncProcessor asyncProcessor, LoadingState state)
    {
        AP = asyncProcessor;

        state.Callback("PreparingBattle", 0);
        yield return null;

        config = (BattleConfig) GameState.sharedData["BattleConfig"];

        playerCamera.GetComponentInChildren<Camera>(true).farClipPlane = 10000000;

        teams = new ShipTeam[] {
            new ShipTeam(0, new IBattleGoal[0], GoalReached),
            new ShipTeam(1, new IBattleGoal[0], GoalReached),
            };

        switch (config.type)
        {
            case BattleType.DeathMatch:
                GenerateDeathMatch();
                break;
            case BattleType.BattleForTheStar:
                GenerateBatlleForTheSun();
                break;
            case BattleType.Escort:
                GenerateEscort();
                break;
        }

        _updaters.Add(new MyMonoBehaviourUpdater(otherObjects));
        _updaters.Add(new LastUpdateShipUpdater());

        base.SetBattle(
            center,
            ships,
            teams,
            _updaters
            );

        base.navigation = navigation;

        AP = null;
        state.Callback("EndLoading", 1f);
    }

    void GenerateDeathMatch()
    {
        center = Vector3.zero;
        distance = config.battleSize.value;

        Vector3 team1Pos = center + Vector3.forward * (distance / 2);
        Vector3 team2Pos = center - Vector3.forward * (distance / 2);

        GenerateShips(team1Pos, team2Pos);
        teams[0].AddGoal(new KillNEnemiesGoal(config.shipsAmount * config.battleDurationMultipier, teams[1]));
        teams[1].AddGoal(new KillNEnemiesGoal(config.shipsAmount * config.battleDurationMultipier, teams[0]));

        foreach (var t in teams)
            t.GetType().GetMethod("Lock", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(t, new object[] { 0, (int)Random.value * int.MaxValue });

        sunSystem = SunSystem.Random(new LineBounds(500, 20000), new LineBounds<int>(1, 13));

        GenerateEnvironment(RandomVector3() * (distance + sunSystem.sun.radius), sunSystem);


        Transform emptyObject = Resources.Load<Transform>("EmptyObject");

        var team1RespawnPoint = new GameObject("Team1RespawnPoint");
        team1RespawnPoint.transform.position = team1Pos;
        var team2RespawnPoint = new GameObject("Team2RespawnPoint");
        team2RespawnPoint.transform.position = team2Pos;

        _updaters.Add(
            new RespawnBehaviour(teams, new Transform[] {
                team1RespawnPoint.transform,
                team2RespawnPoint.transform },
                new float[] { distance, distance},
            25));
    }
    void GenerateBatlleForTheSun()
    {
        center = Vector3.zero;
        distance = config.battleSize.value;
        float halfDistance = distance / 2;

        Vector3 team1Pos = center + Vector3.forward * halfDistance;
        Vector3 team2Pos = center - Vector3.forward * halfDistance;

        float teamOffset = 250;

        GenerateShips(team1Pos + Vector3.forward * (halfDistance + teamOffset), team2Pos - Vector3.forward * (halfDistance + teamOffset));

        sunSystem = SunSystem.Random(new LineBounds(distance * .33f, distance * .33f), new LineBounds<int>(1, 13));

        GenerateEnvironment(center, sunSystem);

        var station1 = new StationInitializerModel(teams[0]).Init(team1Pos - new Vector3(0, 0, 100), Quaternion.LookRotation(Vector3.forward, Vector3.up));
        var station2 = new StationInitializerModel(teams[1]).Init(team2Pos + new Vector3(0, 0, 100), Quaternion.LookRotation(Vector3.forward * -1, Vector3.up));

        {
            var o = station1.AddMyComponent<Obstacle>();
            o.radius = 250;
            navigation.AddObstacle(o);

            o = station2.AddMyComponent<Obstacle>();
            o.radius = 250;
            navigation.AddObstacle(o);
        }
        
        station1.AddMyComponent<SatelliteBehaviour>().SetSun(sun.transform, Vector3.Distance(team1Pos, sun.transform.position), station1.transform.up);
        station2.AddMyComponent<SatelliteBehaviour>().SetSun(sun.transform, Vector3.Distance(team2Pos, sun.transform.position), station1.transform.up);

        ships.Add(station1);
        ships.Add(station2);

        teams[0].AddGoal(new DestroyObjectGoal(station2));
        teams[1].AddGoal(new DestroyObjectGoal(station1));
        foreach (var t in teams)
            t.GetType().GetMethod("Lock", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(t, new object[] { 0, (int)Random.value * int.MaxValue });

        _updaters.Add(new RespawnBehaviour(teams,
        new Transform[] {station1.transform,station2.transform },
        new float[] { distance + teamOffset, distance + teamOffset},
        25));

    }
    void GenerateEscort()
    {
        center = Vector3.zero;
        distance = config.battleSize.value;

        Vector3 team1Pos = center + Vector3.forward * (distance / 2);
        Vector3 team2Pos = center - Vector3.forward * (distance / 2);

        GenerateShips(team1Pos, team1Pos);

        sunSystem = SunSystem.Random(new LineBounds(distance * .33f, distance * .33f), new LineBounds<int>(1, 13));

        Vector3 sunSystemPos = RandomVector3() * (distance + sunSystem.sun.radius);

        GenerateEnvironment(sunSystemPos, sunSystem);
        //Station
        Vector3 stationPos = sunSystemPos + RandomVector3() * (sunSystem.sun.radius * 2);
        var station1 = new StationInitializerModel(teams[0]).Init(stationPos, Quaternion.LookRotation(Vector3.forward, Vector3.up));

        station1.AddMyComponent<SatelliteBehaviour>().SetSun(sun.transform, Vector3.Distance(stationPos, sun.transform.position), station1.transform.up);

        ships.Add(station1);

        //Big ship
        var bigShip = new StationInitializerModel(teams[0]).Init(team1Pos, Quaternion.LookRotation(Vector3.forward, Vector3.up));

        Elf.With(bigShip.AddMyComponent<Obstacle>(),
            o => o.radius = 250,
            o => navigation.AddObstacle(o)
            );

        ships.Add(bigShip);

        teams[0].AddGoal(new EscortObjectGoal(bigShip, station1, 200));
        teams[1].AddGoal(new DestroyObjectGoal(bigShip));
        foreach (var t in teams)
            t.GetType().GetMethod("Lock", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(t, new object[] { 0, (int)Random.value * int.MaxValue });
    }

    void GenerateShips(Vector3 team1Pos, Vector3 team2Pos)
    {
        int playerTeam = Random.Range(0, 2);

        //Get ships from assets. Distribute them in arrays by classes
        var shipModels = GameResources.GetAllShips();

        int shipAmount = config.shipsAmount.value;

        //Get ships distribution by classes { 1, 2, 3 }
        if (config.lightShipsWeight == 0 && config.averageShipsWeight == 0 && config.heavyShipsWeight == 0f)
        {
            config.lightShipsWeight.value = 1;
            config.averageShipsWeight.value = 1;
            config.heavyShipsWeight.value = 1;
        }

        BattleGenerator.ShipIndexPair[] playerShip;
        int playerPosition = Random.Range(0, shipAmount);
        switch(config.playerShip.type)
        {
            case PlayerShip.ShipType.Concrete:
                playerShip = new BattleGenerator.ShipIndexPair[] { new BattleGenerator.ShipIndexPair(shipModels.First(m => m.name == config.playerShip.shipName), playerPosition) };
                break;
            case PlayerShip.ShipType.RandomOfClass:
                playerShip = new BattleGenerator.ShipIndexPair[] { new BattleGenerator.ShipIndexPair(config.playerShip.shipClass, playerPosition) };
                break;
            default:
                playerShip = null;
                break;
        }

        DistributionCube cube = new DistributionCube(500, 500, 200, 5, 10, 2);

        BattleGenerator.GenerateShips(
            BattleGenerator.GenerateShipModels(shipModels, shipAmount, config.lightShipsWeight, config.averageShipsWeight, config.heavyShipsWeight, playerShip),
            BattleGenerator.GenerateDrivers(teams[0], shipAmount, playerTeam == 0 ? playerPosition : -1),
            team1Pos,
            Quaternion.LookRotation(-Vector3.forward, Vector3.up),
            Vector3.up,
            Vector3.right,
            -Vector3.forward,
            cube,
            this.ships
            );

        BattleGenerator.GenerateShips(
            BattleGenerator.GenerateShipModels(shipModels, shipAmount, config.lightShipsWeight, config.averageShipsWeight, config.heavyShipsWeight, playerShip),
            BattleGenerator.GenerateDrivers(teams[1], shipAmount, playerTeam == 1 ? playerPosition : -1),
            team2Pos,
            Quaternion.LookRotation(Vector3.forward, Vector3.up),
            Vector3.up,
            -Vector3.right,
            Vector3.forward,
            cube,
            this.ships
            );
    }

    void GenerateBombFieldSphere(Vector3 position)
    {
        BombBehaviour bomb = Resources.Load<BombBehaviour>(@"Prefabs\Bomb");

        float size = distance;
        float density = 1f / 175f;
        int count = (int)(size * density);
        float interval = size / count;
        Vector3 start = position;
        for (int i = (int) (count * .75f); i < count; i++)
        {
            float radius = i * interval;
            int c = (int) (radius / size * count);
            for (int j = 0; j < c; j++)
            {
                float cosa = Mathf.Cos(3.14f / c * j + i);
                float sina = Mathf.Sin(3.14f / c * j + i);
                for (int k = 0; k < c; k++)
                {
                    float cosb = Mathf.Cos(3.14f / c * k - 1.57f);
                    float sinb = Mathf.Sin(3.14f / c * k - 1.57f);
                    float x = radius * cosa * sinb;
                    float y = radius * sina * sinb;
                    float z = radius * cosb;
                    Instantiate(bomb, start + new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }
    }
    void GenerateBombField(Vector3 position)
    {
        BombBehaviour bomb = Resources.Load<BombBehaviour>(@"Prefabs\Bomb");

        float size = distance;
        float density = 1f / 200f;
        int count = (int) (size * density);
        float interval = size / count;
        Vector3 start = center - position * .6f + new Vector3(size / -2, size / -2, size / -2);
        for (int i = 0; i < count; i++)
        {
            float x = i * interval;
            for (int j = 0; j < count; j++)
            {
                float y = j * interval;
                for (int k = 0; k < count; k++)
                {
                    Instantiate(bomb, start + new Vector3(x, y, k * interval), Quaternion.identity);
                }
            }
        }
    }
    void GenerateEnvironment(Vector3 center, SunSystem sunSystem)
    {
        List<SpaceBody> spaceBodies = new List<SpaceBody>();
        BattleGenerator.GenerateGalaxy(center, sunSystem, navigation, spaceBodies, AP);
        otherObjects.Add(new SunSystemUpdater(spaceBodies));
        sun = spaceBodies.First(b => b is SunBehaviour) as SunBehaviour;
    }

    void GoalReached(int team)
    {
        gameObject.AddComponent<EndBattleBehaviour>()
            .StartEndBattle(playerCamera, sun.transform, EndBattle() );

    }
    Vector3 RandomVector3()
    {
        return new Vector3(Random.value, Random.value, Random.value).normalized;
    }
}

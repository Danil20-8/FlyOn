using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;
using Assets.GameModels;
using Assets.Other;
using Assets.GameModels.Battle;
namespace Assets.GameScripts.Battle
{
    class BattleGenerator
    {
        ShipSystemModel[][] shipModels;

        public BattleGenerator()
        {
            shipModels = GameResources.GetAllShips()
            .GroupBy(s => GameResources.GetShipHull(s.hull.hullName).shipClass)
            .Where(s => s.Key > 0 && s.Key < 4)
            .OrderBy(s => s.Key)
            .Select(s => s.ToArray()).ToArray();
        }

        public static void GenerateShips(IEnumerable<ShipDriver> drivers, Vector3 position, Quaternion rotation, Vector3 up, Vector3 right, Vector3 forward, DistributionCube cube, ICollection<ShipController> outGeneratedShips)
        {
            GenerateShips(drivers.Select(d => d.ship.shipSystem.model), drivers, position, rotation, up, right, forward, cube, outGeneratedShips);
        }

        public static List<ShipController> GenerateShips(IEnumerable<ShipDriver> drivers, Vector3 position, Quaternion rotation, Vector3 up, Vector3 right, Vector3 forward, DistributionCube cube)
        {
            return GenerateShips(drivers.Select(d => d.ship.shipSystem.model), drivers, position, rotation, up, right, forward, cube);
        }

        public static List<ShipController> GenerateShips(IEnumerable<ShipSystemModel> ships, IEnumerable<ShipDriver> drivers, Vector3 position, Quaternion rotation, Vector3 up, Vector3 right, Vector3 forward, DistributionCube cube)
        {
            List<ShipController> result = new List<ShipController>();
            GenerateShips(ships, drivers, position, rotation, up, right, forward, cube, result);
            return result;
        }

        public static void GenerateShips(IEnumerable<ShipSystemModel> ships, IEnumerable<ShipDriver> drivers, Vector3 position, Quaternion rotation, Vector3 up, Vector3 right, Vector3 forward, DistributionCube cube, ICollection<ShipController> outGeneratedShips)
        {
            int shipsAmount = drivers.Count();

            int[] shipsDistribution = Algs.Split(shipsAmount, Enumerable.Range(0, cube.cubesAmount).Select(i => UnityEngine.Random.value).ToArray());
            int n = 0;

            var driver = drivers.GetEnumerator();
            var ship = ships.GetEnumerator();

            foreach(var pos in cube.Distribute(position, up, right, forward))
            {
                for (int m = 0; m < shipsDistribution[n]; m++)
                {
                    driver.MoveNext();
                    ship.MoveNext();

                    var oldShip = driver.Current.ship;
                    var s = new CustomShipInitializerModel(ship.Current, driver.Current)
                            .Init(pos + right * (20 * m), rotation);

                    outGeneratedShips.Add(s);
                }

                n++;
            }
        }

        public static ShipSystemModel[] GenerateShipModels(ShipSystemModel[] models, int amount, float lightWeight, float averageWeight, float heavyWeight, IEnumerable<ShipIndexPair> specialShips = null)
        {
            return GenerateShipModels(models, amount, new float[] { lightWeight, averageWeight, heavyWeight }, specialShips);
        }

        public static ShipSystemModel[] GenerateShipModels(ShipSystemModel[] models, int amount, float[] classWeights, IEnumerable<ShipIndexPair> specialShips = null)
        {
            var ships = models
                .GroupBy(s => GameResources.GetShipHull(s.hull.hullName).shipClass)
                .Where(s => s.Key > 0 && s.Key < 4)
                .OrderBy(s => s.Key)
                .Select(s => s.ToArray()).ToArray();

            int[] kShips = Algs.Split(amount, classWeights);

            var result = new LimitedRandomList<MyRandom<ShipSystemModel>>(ships
                    .Select(kShips, (s, k) => new LimitedProperty<MyRandom<ShipSystemModel>>(new MyRandom<ShipSystemModel>(s), k)).ToArray()
                    ).Select(v => (ShipSystemModel)v).ToArray();

            if (specialShips != null)
            {
                foreach (var s in specialShips)
                {
                    int specialShipClass = s.GetShipClass(ships);
                    ShipSystemModel specialShipModel = s.GetModel(ships);

                    if (result[s.index] == specialShipModel)
                        continue;
                    else
                    {
                        //looking for same model in other position
                        for (int i = 0; i < amount; i++)
                        {
                            // next if index used by other special model
                            if (specialShips.Any(p => p.index == i)) continue;

                            if (result[i] == specialShipModel)
                            {
                                var t = result[i];
                                result[i] = result[s.index];
                                result[s.index] = t;
                                goto NEXT;
                            }
                        }
                    }

                    if (GetShipClass(ships, result[s.index]) == specialShipClass)
                    {
                        result[s.index] = specialShipModel;
                        continue;
                    }
                    else
                    {
                        //looking for same model class in other position;
                        for (int i = 0; i < amount; i++)
                        {
                            // next if index used by other special model
                            if (specialShips.Any(p => p.index == i)) continue;

                            if (GetShipClass(ships, result[i]) == specialShipClass)
                            {
                                result[i] = result[s.index];
                                result[s.index] = specialShipModel;
                                goto NEXT;
                            }
                        }
                    }
                    //Brute setting model
                    result[s.index] = specialShipModel;

                    NEXT:
                    continue;
                }
            }

            return result;
        }
        static int GetShipClass(ShipSystemModel[][] models, ShipSystemModel model)
        {
            for (int i = 0; i < models.Length; i++)
                foreach (var m in models[i])
                    if (m == model)
                        return i + 1;
            throw new System.Exception("special model must be exist in array of other models");
        }
        public struct ShipIndexPair
        {
            public readonly ShipSystemModel model;
            public readonly int shipClass;
            public readonly int index;

            public ShipSystemModel GetModel(ShipSystemModel[][] models)
            {
                if (model != null)
                    return model;
                else
                {
                    return MyRandom.OneOf(models[shipClass - 1]);
                }
            }
            public int GetShipClass(ShipSystemModel[][] models)
            {
                if (model == null)
                    return shipClass;
                else
                {
                    return BattleGenerator.GetShipClass(models, model);
                }
            }
            public ShipIndexPair(ShipSystemModel model, int index)
            {
                this.model = model;
                this.shipClass = 0;
                this.index = index;
            }

            public ShipIndexPair(int shipClass, int index)
            {
                this.model = null;
                this.shipClass = shipClass;
                this.index = index;
            }

        }

        public static ShipDriver[] GenerateDrivers(ShipTeam team, int amount, int playerIndex = -1)
        {
            return Enumerable.Range(0, amount).Select(i => i == playerIndex ? (ShipDriver)new PlayerDriver(team) : new AIDriver(team)).ToArray();
        }

        public static List<SpaceBody> GenerateGalaxy(Vector3 position, SunSystem sunSystem, Navigation navigation)
        {
            List<SpaceBody> bodies = new List<SpaceBody>();
            GenerateGalaxy(position, sunSystem, navigation, bodies);
            return bodies;
        }
        public static void GenerateGalaxy(Vector3 position, SunSystem sunSystem, Navigation navigation, ICollection<SpaceBody> outGeneratedObjects)
        {
            GenerateGalaxy(position, sunSystem, navigation, outGeneratedObjects, null);
        }
        public static void GenerateGalaxy(Vector3 position, SunSystem sunSystem, Navigation navigation, ICollection<SpaceBody> outGeneratedObjects, AsyncProcessor asyncProcessor)
        {
            List<Obstacle> obstacles = new List<Obstacle>();

            var sun = ((SunBehaviour)GameObject.Instantiate(Resources.Load<SunBehaviour>(@"Environment\Sun"), position, Quaternion.identity));
            outGeneratedObjects.Add(sun);

            Color sunColor = new Color(.5f, .5f, .5f) + new Color(Random.value, Random.value, Random.value) * .5f;

            sun.Initialize(sunColor, sunSystem.sun.radius, asyncProcessor);

            sun.AddMyComponent<WorldlGravity>();
            var obstacle = sun.AddMyComponent<Obstacle>();
            obstacle.radius = sunSystem.sun.radius;
            obstacles.Add(obstacle);

            PlanetBehaviour planet = Resources.Load<PlanetBehaviour>(@"Environment\Planet");

            Vector3 axisRight = MyVector.RandomAxis3();
            Vector3 axisUp = Vector3.Cross(sun.transform.up, axisRight);

            foreach (var pl in sunSystem.planets)
            {
                var p = GameObject.Instantiate(planet, sun.transform.position + axisRight * pl.axisOffset, Quaternion.identity);

                p.transform.RotateAround(sun.transform.position, axisUp, Random.value * 360);

                p.Initialize(sun, axisUp, pl.radius, asyncProcessor);

                if (Random.value < .1f)
                    p.InitializeAtmosphere(sunColor, sun.transform.position, sunSystem.sun.radius, pl.radius, asyncProcessor);

                p.AddMyComponent<WorldlGravity>();
                obstacle = p.AddMyComponent<Obstacle>();
                obstacle.radius = pl.radius;
                obstacles.Add(obstacle);

                outGeneratedObjects.Add(p);
            }

            navigation.AddObstacles(obstacles);
        }
    }
}

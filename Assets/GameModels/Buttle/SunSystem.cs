using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.GameModels.Battle.Primiteves;
using Assets.Other;
using Assets.Global;
using UnityEngine;

namespace Assets.GameModels.Battle
{
    public class SunSystem
    {
        public SphereObject sun;
        public SphereObject[] planets;
        
        public float systemRadius { get { return planets[planets.Length - 1].axisOffset + planets[planets.Length - 1].radius; } }

        public SunSystem()
        {

        }

        public static SunSystem Random(LineBounds sunRadius, LineBounds<int> planetsCount)
        {
            var sunSystem = new SunSystem();

            sunSystem.sun = new SphereObject() { radius = UnityEngine.Random.Range(sunRadius.left, sunRadius.right), axisOffset = 0 };

            sunSystem.planets = new SphereObject[UnityEngine.Random.Range(planetsCount.left, planetsCount.right)];

            float planetStep = sunSystem.sun.radius * Constants.worldGravity / sunSystem.planets.Length;
            float currPosition = sunSystem.sun.radius * 1.0f;

            for(int i = 0; i < sunSystem.planets.Length; i++)
            {
                float radius = sunSystem.sun.radius * .05f + sunSystem.sun.radius * .3f * UnityEngine.Random.value;
                currPosition += planetStep * UnityEngine.Random.value + radius;

                sunSystem.planets[i] = new SphereObject();
                sunSystem.planets[i].radius = radius;
                sunSystem.planets[i].axisOffset = currPosition;

                currPosition += radius;
            }

            return sunSystem;
        }
    }
}

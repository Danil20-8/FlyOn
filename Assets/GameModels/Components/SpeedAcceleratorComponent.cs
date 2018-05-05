using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.GameModels.Components
{
    public class SpeedAcceleratorComponent: SystemComponent
    {
        [ComponentInfo("Multiply")]
        float multiply;
        public SpeedAcceleratorComponent(int shipClass): base(shipClass)
        {
            name = "SpeedAccelerator";
            multiply = shipClass + 1;
        }
        void Accelerate(ShipComponentBehaviour master, float rate, float dt)
        {
            master.driver.Accelerate(multiply * rate);
        }
        protected override ComponentActivity[] GetActivities(ShipComponentBehaviour shipComponent)
        {
            return new ComponentActivity[]
            {
                new ContinuousComponentActivity<ShipComponentBehaviour>(shipComponent, () => true, Accelerate)
            };
        }
    }
}

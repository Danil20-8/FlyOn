using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameModels
{
    public class PowerComponent: SystemComponent
    {
        [ComponentInfo("Energy")]
        public float energy { get; protected set; }
        public PowerComponent(int shipClass): base(shipClass)
        {
            energy = 1200 * UnityEngine.Mathf.Pow(3, shipClass);
            name = "Reactor";
        }
        public virtual float OnPulse()
        {
            return energy;
        }

    }
}

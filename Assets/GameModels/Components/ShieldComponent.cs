using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameModels.Components
{
    public class ShieldComponent: SystemComponent
    {
        public ShieldComponent(int shipClass): base(shipClass)
        {
            name = "Shield";
        }
        void UpdateShield(ShieldComponentBehaviour shield, float rate)
        {
            shield.UpdateShield(rate);
        }
        protected override void OnDisabled(ShipComponentBehaviour shipComponent)
        {
            ((ShieldComponentBehaviour)shipComponent).Disable();
        }
        /*protected override ComponentActivity[] GetActivities(ShipComponentBehaviour shipComponent)
        {
            var shield = (ShieldComponentBehaviour)shipComponent;
            return new ComponentActivity[]
            {
                new EnergyChangeComponentActivity<ShieldComponentBehaviour>(shield, UpdateShield)
            };
        }*/
    }
}

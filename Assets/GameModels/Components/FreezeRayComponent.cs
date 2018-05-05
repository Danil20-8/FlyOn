using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameModels.Components
{
    public class FreezeRayComponent: SystemComponent
    {
        [ComponentInfo("Distance")]
        public float distance { get; private set; }

        public FreezeRayComponent(int shipClass): base(shipClass)
        {
            name = "FreezeRay";
            distance = 400 + 400 * shipClass;
        }
        void Direct(FreezeRayComponentBehaviour master, float rate, float timeSpend)
        {
            master.Direct(rate * 45f * timeSpend);
        }
        void Fire(FreezeRayComponentBehaviour master, float rate, float dt)
        {
            master.Fire(distance * rate);
        }
        protected override ComponentActivity[] GetActivities(ShipComponentBehaviour shipComponent)
        {
            FreezeRayComponentBehaviour rayGun = (FreezeRayComponentBehaviour)shipComponent;
            return new ComponentActivity[]
            {
                new ContinuousComponentActivity<FreezeRayComponentBehaviour>(rayGun, rayGun.IsDirecting, Direct),
                new ContinuousComponentActivity<FreezeRayComponentBehaviour>(rayGun, rayGun.IsFiring, Fire)
            };
        }
    }
}

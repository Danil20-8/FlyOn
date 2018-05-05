using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameModels.Components
{
    public class EngineComponent : SystemComponent
    {
        [ComponentInfo("Speed")]
        public float movementSpeed { get; protected set; }
        public float rotationSpeed { get; protected set; }
        public EngineComponent(int shipClass) : base(shipClass)
        {
            movementSpeed = 6f + 1f * shipClass;
            rotationSpeed = 30f - 6f * shipClass;
            name = "Engine";
        }

        protected override void OnEnabled(ShipComponentBehaviour shipComponent)
        {
            ((EngineComponentBehaviour)shipComponent).Enable();
        }
        protected override void OnDisabled(ShipComponentBehaviour shipComponent)
        {
            ((EngineComponentBehaviour)shipComponent).Disable();
        }

        void Move(ShipComponentBehaviour master, float rate, float timeSpend)
        {
            rate *= timeSpend;
            master.driver.Move(movementSpeed * rate);
            master.driver.Rotate(rotationSpeed * rate);
        }
        protected override ComponentActivity[] GetActivities(ShipComponentBehaviour shipComponent)
        {
            return new ComponentActivity[]
            {
                new ContinuousComponentActivity<ShipComponentBehaviour>(shipComponent, () => true, Move)
            };
        }
    }
}

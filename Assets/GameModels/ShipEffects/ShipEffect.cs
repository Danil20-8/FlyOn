using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameModels.ShipEffects
{
    public abstract class ShipEffect
    {
        public abstract void Apply(ShipState state);
        public abstract void Cancel(ShipState state);
    }
    public class FreezeEffect : ShipEffect
    {
        public override void Apply(ShipState state)
        {
            state.updateLinks = false;
        }

        public override void Cancel(ShipState state)
        {
            state.updateLinks = true;
        }
    }
    public class ShipState
    {
        public bool updateLinks = true;
        public bool updateDriver = true;
        public bool takeDamage = true;

        public void Reset()
        {
            updateLinks = true;
            updateDriver = true;
            takeDamage = true;
        }
    }
}

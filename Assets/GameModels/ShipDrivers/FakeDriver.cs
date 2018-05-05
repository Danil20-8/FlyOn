using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameModels.ShipDrivers
{
    public class FakeDriver : ShipDriver
    {
        bool enabled;

        public FakeDriver(bool enabled = false)
            :base(null)
        {
            this.enabled = enabled;
        }
        protected override void OnSetShip()
        {
            ship.enabled = enabled;
        }

    
    }
}

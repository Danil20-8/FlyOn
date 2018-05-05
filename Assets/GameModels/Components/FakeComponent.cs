using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameModels.Components
{
    public class FakeComponent : SystemComponent
    {
        public FakeComponent(int shipClass = 0): base(shipClass)
        {
            name = "Fake";
        }
    }
}

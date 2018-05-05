using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib;
namespace Assets.GameModels
{
    public class PowerLink : SystemLink
    {
        public PowerLink(PowerComponent component): base(component)
        {
            priority = 0;
        }

        public void Pulse()
        {
            if (alive && enabled)
                OnPulse();
        }
        public void Diagnose()
        {
            // when removing or adding link
            if (systemChanged)
            {
                RefreshCache();
                systemChanged = false;
            }

            // when link gets damage
            if (elementChanged)
            {
                OnPowerConnecteds(health);
                elementChanged = false;
            }
        }
        protected virtual void OnPulse()
        {
            // when removing or adding link
            if (systemChanged)
            {
                RefreshCache();
                systemChanged = false;
            }

            // when link gets damage
            if (elementChanged)
            {
                OnPowerConnecteds(health);
                elementChanged = false;
            }

            Activate();
        }
    }

}

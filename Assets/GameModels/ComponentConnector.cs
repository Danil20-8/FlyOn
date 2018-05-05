using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;

namespace Assets.GameModels
{
    public class ComponentConnector
    {
        SystemComponent component;
        ComponentActivity[] activities;
        Action disabledEvent;
        Action enabledEvent;


        public void SetComponent(SystemComponent component, ComponentActivity[] activities, Action disabledEvent, Action enabledEvent)
        {
            this.component = component;
            this.activities = activities;
            this.disabledEvent = disabledEvent;
            this.enabledEvent = enabledEvent;

            foreach (var a in this.activities)
                a.SetConnector(this);
        }

        public void SetComponent(ShipComponentBehaviour component, ComponentActivity[] activities, Action disabledEvent, Action enabledEvent)
        {

        }

        public void OnPower(float rate)
        {
            foreach (var a in activities)
                a.OnPower(rate);
        }
        public void OnDisabled()
        {
            disabledEvent();
        }
        public void OnEnabled()
        {
            enabledEvent();
        }
    }
    public abstract class ComponentActivity
    {
        protected ComponentConnector connector;

        public void SetConnector(ComponentConnector connector)
        {
            if (this.connector != null)
                throw new Exception("Connector already seted");

            this.connector = connector;
        }


        public abstract void OnPower(float rate);
    }
    public class TimerComponentActivity<T> : ComponentActivity where T : ShipComponentBehaviour
    {
        float timeToOn;
        float lastActive = 0;
        Func<bool> activator;
        Action<T, float> doWork;
        T master;

        float current;

        public TimerComponentActivity(T master, float timeToOn, Func<bool> activator, Action<T, float> doWork)
        {
            this.timeToOn = timeToOn;
            this.activator = activator;
            this.doWork = doWork;
            this.master = master;

            current = timeToOn;
        }

        public override void OnPower(float rate)
        {
            current += BattleBehaviour.deltaTime;
            if (current >= timeToOn)
            {
                if (activator())
                {
                    doWork(master, rate);
                    current = 0f;
                }
            }
        }
    }
    public class ContinuousComponentActivity<T> : ComponentActivity where T : ShipComponentBehaviour
    {
        Func<bool> activator;
        Action<T, float, float> doWork;
        T master;
        public ContinuousComponentActivity(T master, Func<bool> activator, Action<T, float, float> doWork)
        {
            this.activator = activator;
            this.doWork = doWork;
            this.master = master;
        }

        public override void OnPower(float rate)
        {
            if (activator())
                doWork(master, rate, BattleBehaviour.deltaTime);
        }
    }

    public class EnergyChangeComponentActivity<T> : ComponentActivity
    {
        float lastRate = 0;
        Action<T, float> doWork;
        T master;
        public EnergyChangeComponentActivity(T master, Action<T, float> doWork)
        {
            this.doWork = doWork;
            this.master = master;
        }
        public override void OnPower(float rate)
        {
            if (lastRate != rate)
            {
                lastRate = rate;
                doWork(master, rate);
            }
        }
    }
}

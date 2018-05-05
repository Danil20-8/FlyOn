using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Assets.Other;
using MyLib;
using UnityEngine;
namespace Assets.GameModels
{
    public class SystemLink : ITreeable<SystemLink>
    {

        public bool alive { get { return _health > 0; } }
        public bool enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    if (enabledInBranch)
                    {
                        if (!_enabled && value)
                        {
                            _enabled = value;
                            OnEnable();
                        }
                        else {
                            _enabled = value;
                            OnDisable();
                        }

                        systemChanged = true;
                        elementChanged = true;
                    }
                    else
                        _enabled = value;
                }
            }
        }
        bool _enabled = false;
        public bool enabledInBranch
        {
            get
            {
                var r = root;
                while (r != null)
                {
                    if (!r.enabled)
                        return false;
                    r = r.root;
                }
                return true;
            }
        }
        public float health { get { return _health / maxHealth; } }
        float _health;
        public readonly float maxHealth;

        protected float priority = 1f;

        protected bool elementChanged { get { return superRoot._elementChanged; } set { superRoot._elementChanged = value; } }
        protected bool systemChanged { get { return superRoot._systemChanged; } set { superRoot._systemChanged = value; } }
        bool _elementChanged = false;
        bool _systemChanged = false;

        SystemLink superRoot { get { return _superRoot != null ? _superRoot : this; } }
        SystemLink _superRoot;
        public SystemLink root { get; set; }

        SystemLink[] cache = new SystemLink[0];

        public float lastEnergy { get; private set; }

        public readonly SystemComponent item;
        ComponentConnector connector;

        List<SystemLink> links = new List<SystemLink>();
        public IEnumerable<SystemLink> childs { get { return links; } }

        public SystemLink(SystemComponent component)
        {
            item = component;
            maxHealth = new float[] { 1, 2, 5, 10, 20 }[component.shipClass];
            _health = maxHealth;
        }
        //Activating in branch
        void OnEnable()
        {
            //you can't enable next links if you are disabled
            if (_enabled)
            {
                connector.OnEnabled();
                foreach (var l in links)
                    l.OnEnable();
            }
        }
        //Disabling in branch
        void OnDisable()
        {
            //root disabled then branch disabled
            connector.OnDisabled();
            foreach (var l in links)
                l.OnDisable();
        }
        protected void RefreshCache()
        {
            cache = this.ByElements(i => i.enabled).Where(l => l.alive).ToArray();
        }

        protected void OnPowerConnecteds(float power)
        {
            foreach (var l in cache)
                l.lastEnergy = power * l.health;
        }
        protected void Activate()
        {
            foreach (var l in cache)
                l.connector.OnPower(l.lastEnergy);
        }

        public void SetDamage(float damage)
        {
            if (alive)
            {
                _health -= damage;
                elementChanged = true;
                if (_health < 0)
                {
                    _health = 0;
                    connector.OnDisabled();
                    systemChanged = true;
                }
            }
        }

        public void SetMaster(ShipComponentBehaviour master)
        {
            connector = new ComponentConnector();
            item.SetConnector(connector, master);
        }

        public SystemLink Clone()
        {
            return this.BuildTree(n => Pack(item));
        }
        public Info GetInfo()
        {
            return new Info(this);
        }

        public void AddChild(SystemLink child)
        {
            child._superRoot = superRoot;
            links.Add(child);

            elementChanged = true;
            systemChanged = true;
        }

        public void RemoveChild(SystemLink child)
        {
            links.Remove(child);

            elementChanged = true;
            systemChanged = true;
        }

        public static SystemLink Pack(SystemComponent component)
        {
            if (component is PowerComponent)
                return new PowerLink((PowerComponent)component);
            else
                return new SystemLink(component);
        }

        public struct Info
        {
            public bool alive { get { return link.alive; } }
            public bool enabled { get { return link.enabled; } }
            public string name { get { return link.item.name; } }
            public float lastEnergy { get { return link.lastEnergy; } }
            SystemLink link;
            public Info(SystemLink link)
            {
                this.link = link;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MyLib.Serialization;
using Assets.GameModels.Phisical;
using Assets.Global;
using Assets.Other.Special;

namespace Assets.GameModels
{
    public abstract class SystemComponent : ICloneable<SystemComponent>
    {
        [ComponentInfo("Class")]
        public string sShipClass { get {
                switch (shipClass)
                {
                    case 0:
                        return Localization.GetGlobalString("Terrestrial");
                    case 1:
                        return Localization.GetGlobalString("Light");
                    case 2:
                        return Localization.GetGlobalString("Average");
                    case 3:
                        return Localization.GetGlobalString("Heavy");
                    case 4:
                        return Localization.GetGlobalString("Stationary");
                    default:
                        return "Miss";
                }
            } }

        LString _lname;
        public string lName { get { return _lname; } private set { _lname = (LString)value; } }
        string _name;
        public string name { get { return _name; } protected set { _name = value; lName = value; } }

        [ConstructorArg(0)]
        public readonly int shipClass;

        public SystemComponent(int shipClass)
        {
            name = "Unknown component";
            this.shipClass = shipClass;
        }

        public void SetConnector(ComponentConnector connector, ShipComponentBehaviour shipComponent)
        {
            connector.SetComponent(this, GetActivities(shipComponent), () => OnDisabled(shipComponent), () => OnEnabled(shipComponent));
        }
        protected virtual ComponentActivity[] GetActivities(ShipComponentBehaviour shipComponent)
        {
            return new ComponentActivity[0];
        }
        protected virtual void OnDisabled(ShipComponentBehaviour shipComponent)
        {

        }
        protected virtual void OnEnabled(ShipComponentBehaviour shipComponent)
        {

        }
        public virtual ComponentInfo GetInfo()
        {
            ComponentInfo info = new ComponentInfo();
            PropertyInfo[] members = GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(ComponentInfoAttribute), true).Any()).ToArray();
            foreach (var m in members)
            {
                var attr = (ComponentInfoAttribute)m.GetCustomAttributes(typeof(ComponentInfoAttribute), true).First();
                info.AddInfo(new ParamInfo()
                {
                    Name = attr.name != "" ? attr.name : m.Name,
                    Value = m.GetValue(this, null).ToString(),
                    Discription = attr.discription
                });
            }
            return info;
        }
        public void SetConfig(ConfigComponent config)
        {
            foreach (var f in GetType().GetProperties().Where(f => f.GetCustomAttributes(typeof(ConfigFieldAttribute), true).Any()))
                f.SetValue(this, config.GetField(f.Name), null);
        }
        public ConfigComponent GetConfig()
        {
            ConfigComponent config = new ConfigComponent();
            foreach (var f in GetType().GetProperties().Where(f => f.GetCustomAttributes(typeof(ConfigFieldAttribute), true).Any()))
                config.AddField(f.Name, f.GetValue(this, null));
            return config;
        }
        public ConfigComponent GetDefaultConfig()
        {
            ConfigComponent config = new ConfigComponent();
            foreach (var f in GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(ConfigFieldAttribute), true).Any()))
                config.AddField(f.Name, ((ConfigFieldAttribute)f.GetCustomAttributes(typeof(ConfigFieldAttribute), true)[0]).defaultValue);
            return config;
        }
        public SystemComponent Clone()
        {
            return (SystemComponent)GetType().GetConstructor(new Type[] { typeof(int) })
                .Invoke(new object[] { shipClass});
        }
    }
}

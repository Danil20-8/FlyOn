using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MyLib.Serialization;
using MyLib.Serialization.Binders;
namespace Assets.GameModels
{
    public class ConfigComponent
    {
        [Addon]
        [SerializeBinder(typeof(DictionaryBinder<string, object>))]
        Dictionary<string, object> fields = new Dictionary<string, object>();

        public void AddField(string info, object value)
        {
            fields.Add(info, value);
        }

        public object GetField(string name)
        {
            var field = fields.Where(f => f.Key == name);
            if (field.Any())
                return field.First().Value;
            else
                return null;
        }
        public Type GetFieldType(string name)
        {
            var field = fields.Where(f => f.Key == name);
            if (field.Any())
                return field.First().Value.GetType();
            else
                return null;
        }
        public string[] GetNames()
        {
            return fields.Select(f => f.Key).ToArray();
        }
        public void SetField(string name, object value)
        {
            foreach(var f in fields)
                if(f.Key == name)
                {
                    fields[f.Key] = value;
                    return;
                }
            throw new Exception(string.Format("Field with name {0}'s not found", name));
        }
    }

    public class ConfigFieldAttribute : Attribute
    {
        public readonly object defaultValue;
        public ConfigFieldAttribute(object defaultValue)
        {
            this.defaultValue = defaultValue;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameModels
{
    public class ComponentInfo : IEnumerable<ParamInfo>
    {
        List<ParamInfo> info = new List<ParamInfo>();
        public ComponentInfo()
        {

        }

        public void AddInfo(ParamInfo info)
        {
            this.info.Add(info);
        }

        public IEnumerator<ParamInfo> GetEnumerator()
        {
            return ((IEnumerable<ParamInfo>)info).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ParamInfo>)info).GetEnumerator();
        }
    }
    public struct ParamInfo
    {
        public string Name;
        public string Value;
        public string Discription;
    }
    public class ComponentInfoAttribute : Attribute
    {
        public readonly LString name;
        public readonly LString discription;

        public ComponentInfoAttribute(string name = "", string discription = "")
        {
            this.name = (LString)name;
            this.discription = (LString)discription;
        }
    }
}

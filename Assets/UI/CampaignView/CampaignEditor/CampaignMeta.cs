using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.GameModels.Campaign;
using MyLib.Serialization;

namespace Assets.UI.CampaignView.CampaignEditor
{
    public class CampaignMeta
    {
        [Addon]
        public Campaign campaign;
        [Addon]
        public List<int[]> stages;
        [Addon]
        private SVector3[] _positions { get { return positions.Select(p => new SVector3(p)).ToArray(); } set { positions = value.Select(p => (Vector3)p).ToArray(); } }

        public Vector3[] positions;
    }

    [Serializable]
    public struct SVector3
    {
        public float x, y, z;

        public SVector3(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public static implicit operator Vector3(SVector3 svec)
        {
            return new Vector3(svec.x, svec.y, svec.z);
        }
        public static implicit operator SVector3(Vector3 vec)
        {
            return new SVector3(vec);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Global
{
    public static class Constants
    {
        public const float worldGravity = 9f;
        public static float deg_15 = Mathf.Cos(15 * Mathf.Deg2Rad); // .966f;
        public static readonly int shipItemLayer = LayerMask.NameToLayer("ShipItem");
        public static readonly int defaultLayer = LayerMask.NameToLayer("Default");

        public static readonly int shotMask = LayerMask.GetMask("Default", "ShipItem");
        public static readonly int moveMask = LayerMask.GetMask("Default");
    }
}

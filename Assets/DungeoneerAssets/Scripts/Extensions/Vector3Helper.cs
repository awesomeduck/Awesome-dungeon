using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityEngine
{
    public static class Vector3Helper
    {

        public static Vector3 Round(this Vector3 vec, int decimals)
        {
            vec.Set((float)Math.Round(vec.x, decimals),
                (float)Math.Round(vec.y, decimals),
                (float)Math.Round(vec.z, decimals));
            return vec;
        }

        public static Vector3 RoundLaterals(this Vector3 vec, int decimals)
        {
            vec.Set((float)Math.Round(vec.x, decimals),
                vec.y, 
                (float)Math.Round(vec.z, decimals)); 
            return vec; 
        }
    }
}

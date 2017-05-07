using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class StatWeight
    {
        public StatType Type { get; set; }
        /// <summary>
        /// 0...1
        /// </summary>
        public float Weight { get; set; }

        public float AdjustedWeight { get; set; }
    }
}
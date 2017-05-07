using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class Stats
    {
        public int HP { get; set; }
        public int MP { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int Str { get; set; }
        public int Vit { get; set; }
        public int Int { get; set; }
        public int Agi { get; set; }

    }

    public enum StatType
    {
        Str, Vit, Int, Agi
    }
}
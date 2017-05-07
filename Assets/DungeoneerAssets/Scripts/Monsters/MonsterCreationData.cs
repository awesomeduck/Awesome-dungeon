using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class MonsterCreationData
    {
        public MonsterSprite Sprite { get; set; }
        public MonsterType Type { get; set; }
        public double PowerScale { get; set; }
        public Location Location { get; set; }

        public int? XP { get; set; }
        public int? Gold { get; set; }
        public string Name { get; set; }
    }
}
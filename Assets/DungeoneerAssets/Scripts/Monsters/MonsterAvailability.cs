using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class MonsterAvailability
    {
        public MonsterType Type { get; set; }
        public float EncounterChance { get; set; }
        public float AdjustedEncounterChance { get; set; }
        public List<MonsterSprite> Sprites { get; set; }

        public MonsterAvailability()
        {
            this.Sprites = new List<MonsterSprite>();
        }
    }

    public class MonsterSprite
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public static class MonsterStatWeights
    {
        public static Dictionary<MonsterType, StatWeights> statWeights;

        static MonsterStatWeights()
        {
            statWeights = new Dictionary<MonsterType, StatWeights>();
            statWeights.Add(MonsterType.Fighter, new StatWeights());
            statWeights.Add(MonsterType.Mage, new StatWeights());
            statWeights.Add(MonsterType.Tank, new StatWeights());
            statWeights.Add(MonsterType.GlassCannon, new StatWeights());
            statWeights.Add(MonsterType.Balanced, new StatWeights());
            statWeights.Add(MonsterType.SpeedDemon, new StatWeights());

            statWeights[MonsterType.Fighter].Add(new StatWeight { Type = StatType.Str, Weight = .65f });
            statWeights[MonsterType.Fighter].Add(new StatWeight { Type = StatType.Vit, Weight = .25f });
            statWeights[MonsterType.Fighter].Add(new StatWeight { Type = StatType.Agi, Weight = .1f });

            statWeights[MonsterType.Mage].Add(new StatWeight { Type = StatType.Vit, Weight = .3f });
            statWeights[MonsterType.Mage].Add(new StatWeight { Type = StatType.Int, Weight = .6f });
            statWeights[MonsterType.Mage].Add(new StatWeight { Type = StatType.Agi, Weight = .1f });

            statWeights[MonsterType.Tank].Add(new StatWeight { Type = StatType.Str, Weight = .5f });
            statWeights[MonsterType.Tank].Add(new StatWeight { Type = StatType.Vit, Weight = .5f });

            statWeights[MonsterType.GlassCannon].Add(new StatWeight { Type = StatType.Str, Weight = 0f });
            statWeights[MonsterType.GlassCannon].Add(new StatWeight { Type = StatType.Vit, Weight = .3f });
            statWeights[MonsterType.GlassCannon].Add(new StatWeight { Type = StatType.Int, Weight = .7f });
            statWeights[MonsterType.GlassCannon].Add(new StatWeight { Type = StatType.Agi, Weight = 0f });

            statWeights[MonsterType.Balanced].Add(new StatWeight { Type = StatType.Str, Weight = .45f });
            statWeights[MonsterType.Balanced].Add(new StatWeight { Type = StatType.Vit, Weight = .20f });
            statWeights[MonsterType.Balanced].Add(new StatWeight { Type = StatType.Int, Weight = .20f });
            statWeights[MonsterType.Balanced].Add(new StatWeight { Type = StatType.Agi, Weight = .15f });

            statWeights[MonsterType.SpeedDemon].Add(new StatWeight { Type = StatType.Str, Weight = .20f });
            statWeights[MonsterType.SpeedDemon].Add(new StatWeight { Type = StatType.Vit, Weight = .20f });
            statWeights[MonsterType.SpeedDemon].Add(new StatWeight { Type = StatType.Agi, Weight = .60f });

            foreach (var weights in statWeights.Values)
                weights.CalculatedAdjustedWeights();
        }
    }
}
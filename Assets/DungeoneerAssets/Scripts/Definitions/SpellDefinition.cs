using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class SpellDefinition : DefinitionBase
    {
        public int MPCost { get; set; }
        public Func<int, int> CalculatePower { get; set; }
        public StatRequirement StatRequirement { get; set; }
        public Func<Player, bool> CanUse { get; set; }
        public AttackType AttackType { get; set; }

        /// <summary>
        /// Check if the passed in stats are greater than the stored stats
        /// </summary>
        /// <param name="rightSide"></param>
        /// <returns></returns>
        public bool RequirementsMet(StatRequirement stats)
        {
            if (this.StatRequirement == null)
                return true;

            return stats.Str >= this.StatRequirement.Str &&
                stats.Vit >= this.StatRequirement.Vit &&
                stats.Int >= this.StatRequirement.Int &&
                stats.Agi >= this.StatRequirement.Agi;
        }

        public bool RequirementsMet(Stats stats)
        {
            if (this.StatRequirement == null)
                return true;

            return stats.Str >= this.StatRequirement.Str &&
                stats.Vit >= this.StatRequirement.Vit &&
                stats.Int >= this.StatRequirement.Int &&
                stats.Agi >= this.StatRequirement.Agi;
        }

    }
}
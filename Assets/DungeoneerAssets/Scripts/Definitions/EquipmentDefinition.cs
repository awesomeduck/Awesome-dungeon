using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class EquipmentDefinition : DefinitionBase
    {
        public EquipmentValue Value { get; set; }
        public EquipmentType Type { get; set; }
        public StatRequirement StatRequirements { get; set; }

        public bool StatRequirementsMet(Player player)
        {
            if (this.StatRequirements == null)
                return true;

            return player.Str >= this.StatRequirements.Str &&
                player.Vit >= this.StatRequirements.Vit &&
                player.Int >= this.StatRequirements.Int &&
                player.Agi >= this.StatRequirements.Agi;
        }
    }
}
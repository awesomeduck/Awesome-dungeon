using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class EquipmentValue
    {
        public int Str { get; set; }
        public int Vit { get; set; }
        public int Int { get; set; }
        public int Agi { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }

        public int PhysicalAttack { get; set; }
        public int PhysicalDefense { get; set; }
        public int MagicalAttack { get; set; }
        public int MagicalDefense { get; set; }

        public EquipmentValue()
        { }

        public EquipmentValue(int physAtk, int physDef, int magAtk, int magDef)
        {
            this.PhysicalAttack = physAtk;
            this.PhysicalDefense = physDef;
            this.MagicalAttack = magAtk;
            this.MagicalDefense = magDef;
        }
    }
}
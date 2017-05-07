using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class SpeedDemonMonster : MonsterBase
    {
        public SpeedDemonMonster()
            : base()
        {
            this.Stats.Str = 3;
            this.Stats.Vit = 3;
            this.Stats.Agi = 6;
            this.Stats.Int = 0;
            this.Stats.MaxHP = this.Stats.HP = 15;
            this.Stats.MaxMP = this.Stats.MP = 0;
            this.CalculateDerivedStats();
            this.MonsterName = "SpeedDemon";
        }

        public override CharacterAction EnemyTurn()
        {
            /*
             * Same as fighter, just attack constantly. 
             */
            return this.PhysicalAttackAction();
        }
    }
}
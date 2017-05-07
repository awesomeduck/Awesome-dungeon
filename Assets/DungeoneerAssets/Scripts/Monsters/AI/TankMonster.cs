using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class TankMonster : MonsterBase
    {
        public TankMonster()
            : base()
        {
            this.Stats.Str = 4;
            this.Stats.Vit = 6;
            this.Stats.Agi = 4;
            this.Stats.Int = 0;
            this.Stats.MaxHP = this.Stats.HP = 20;
            this.Stats.MaxMP = this.Stats.MP = 0;
            this.CalculateDerivedStats();
            this.MonsterName = "Tank";
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
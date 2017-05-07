using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class GlassCannonMonster : MonsterBase
    {
        public GlassCannonMonster()
            : base()
        {
            this.Stats.Str = 2;
            this.Stats.Vit = 2;
            this.Stats.Agi = 2;
            this.Stats.Int = 6;
            this.Stats.MaxHP = this.Stats.HP = 15;
            this.Stats.MaxMP = this.Stats.MP = 20;
            this.CalculateDerivedStats();
            this.MonsterName = "GlassCannon";
        }

        public override void DetermineSpells()
        {
            this.availableSpells = new List<string>();
            this.availableSpells.Add("Fire");
        }


        public override CharacterAction EnemyTurn()
        {
            /*
             * use spells until mp is drained, dont heal, just attack constantly. 
             */

            bool canCast = this.CanCastSpell("Fire");
            if (canCast)
                return this.AttackSpellAction();

            return this.PhysicalAttackAction();
        }
    }
}
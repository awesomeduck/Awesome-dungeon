using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class FighterMonster : MonsterBase
    {
        public FighterMonster()
            : base()
        {
            this.Stats.Str = 5;
            this.Stats.Vit = 5;
            this.Stats.Agi = 4;
            this.Stats.Int = 0;
            this.Stats.MaxHP = this.Stats.HP = 15;
            this.Stats.MaxMP = this.Stats.MP = 0;
            this.CalculateDerivedStats();
            this.MonsterName = "Fighter";
        }

        public override CharacterAction EnemyTurn()
        {
            return this.PhysicalAttackAction();
        }
    }
}
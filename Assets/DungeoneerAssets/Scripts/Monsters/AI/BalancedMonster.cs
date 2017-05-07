using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class BalancedMonster : MonsterBase
    {
        public BalancedMonster()
            : base()
        {
            this.Stats.Str = 4;
            this.Stats.Vit = 4;
            this.Stats.Agi = 4;
            this.Stats.Int = 4;
            this.Stats.MaxHP = 15;
            this.Stats.MaxMP = 15;
            this.Stats.HP = 15;
            this.Stats.MP = 15;
            this.CalculateDerivedStats();
            this.MonsterName = "Balanced";
        }

        public override void DetermineSpells()
        {
            bool canHeal = false;
            bool canShield = false;
            bool canBarrier = false;
            int size = 1;

            if (UnityEngine.Random.Range(0f, 1f) <= 0.7f)
            {
                if (SpellDefinitions.Spells["Heal"].RequirementsMet(this.Stats))
                {
                    canHeal = true;
                    size++;
                }
            }
            if (UnityEngine.Random.Range(0f, 1f) <= 0.3f)
            {
                if (SpellDefinitions.Spells["Shield"].RequirementsMet(this.Stats))
                {
                    canShield = true;
                    size++;
                }
            }
            if (UnityEngine.Random.Range(0f, 1f) <= 0.3f)
            {
                if (SpellDefinitions.Spells["Barrier"].RequirementsMet(this.Stats))
                {
                    canBarrier = true;
                    size++;
                }
            }

            this.availableSpells = new List<string>();
            this.availableSpells.Add("Fire");
            if (canHeal)
                this.availableSpells.Add("Heal");
            if (canShield)
                this.availableSpells.Add("Shield");
            if (canBarrier)
                this.availableSpells.Add("Barrier");
        }


        public override CharacterAction EnemyTurn()
        {
            /*
             * First off evaluate int vs str to determine preferred attack style
             * If they're close to each other switch back and forth, otherwise use the larger one first and in the case of int being stronger fall back to physical once down to the last heal or 2. 
             * If physical is higher but enemy has heal (make chance of having it based on int), then use heal when Hp <= 25%. 
             */
            bool canHeal = this.CanCastSpell("Heal");
            float hpPercent = (float)this.Stats.HP / (float)this.Stats.MaxHP;
            if (hpPercent <= .33f && canHeal)
                return this.HealAction();

            if (UnityEngine.Random.Range(0f, 1f) >= .7f)
                return this.PhysicalAttackAction();
            else if (this.CanCastSpell("Fire"))
                return this.AttackSpellAction();
            else return this.PhysicalAttackAction();
        }
    }
}
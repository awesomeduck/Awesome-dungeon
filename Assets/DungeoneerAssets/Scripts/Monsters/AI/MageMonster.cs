using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class MageMonster : MonsterBase
    {
        public MageMonster()
            : base()
        {
            this.Stats.Str = 2;
            this.Stats.Vit = 4;
            this.Stats.Agi = 4;
            this.Stats.Int = 4;
            this.Stats.MaxHP = this.Stats.HP = 15;
            this.Stats.MaxMP = this.Stats.MP = 20;
            this.CalculateDerivedStats();
            this.MonsterName = "Mage";
        }

        public override void DetermineSpells()
        {
            bool canHeal = false;
            bool canShield = false;
            bool canBarrier = false;
            int size = 1;

            if (UnityEngine.Random.Range(0f, 1f) >= 0.7f)
            {
                if (SpellDefinitions.Spells["Heal"].RequirementsMet(this.Stats))
                {
                    canHeal = true;
                    size++;
                }
            }
            if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
            {
                if (SpellDefinitions.Spells["Shield"].RequirementsMet(this.Stats))
                {
                    canShield = true;
                    size++;
                }
            }
            if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
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
             * Balanced magic casting between damage, healing and support.
             */

            float hpPercent = (float)this.Stats.HP / (float)this.Stats.MaxHP;
            bool canCastHeal = this.CanCastSpell("Heal");
            bool canCastFire = this.CanCastSpell("Fire");
            bool canCastShield = this.CanCastSpell("Shield");
            bool canCastBarrier = this.CanCastSpell("Barrier");

            if (canCastShield && this.PlayerUsesPhysicalAttacks)
                return this.ShieldAction();

            if (canCastBarrier && this.PlayerUsesMagicalAttacks)
                return this.BarrierAction();

            if (!canCastHeal && !canCastFire)
                return this.PhysicalAttackAction();
            else
            {
                if (hpPercent < .60 && canCastHeal)
                {
                    int chance = UnityEngine.Random.Range(0, 100);
                    if (chance <= 50)
                        return this.HealAction();
                }

                if (hpPercent < .25 && canCastHeal)
                    return this.HealAction();

                if (canCastFire)
                    return this.AttackSpellAction();
                else return this.PhysicalAttackAction();
            }
        }
    }
}
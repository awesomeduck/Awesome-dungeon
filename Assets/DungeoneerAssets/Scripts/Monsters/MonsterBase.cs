using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public abstract class MonsterBase : MonoBehaviour
    {
        public static readonly string[] possibleSpells = new string[] { "Fire", "Heal", "Shield", "Barrier" };
        public string MonsterName { get; set; }

        protected List<string> availableSpells;

        public MonsterType MonsterType { get; set; }
        public Stats Stats { get; set; }

        public int PhysicalAttack { get; protected set; }
        public int PhysicalDefense { get; protected set; }
        public int MagicalAttack { get; protected set; }
        public int MagicalDefense { get; protected set; }

        public int XP { get; set; }
        public int Gold { get; set; }

        public bool PlayerUsesPhysicalAttacks { get; set; }
        public bool PlayerUsesMagicalAttacks { get; set; }
        public List<TimedStatModifier> StatMods { get; private set; }

        public MonsterBase()
        {
            this.Stats = new Stats();
            this.StatMods = new List<TimedStatModifier>();
        }

        public virtual void CalculateDerivedStats()
        {
            this.PhysicalAttack = this.Stats.Str;
            this.MagicalAttack = this.Stats.Int;
            this.PhysicalDefense = (int)(this.Stats.Vit * .5f);
            this.MagicalDefense = (int)(this.Stats.Int * .5f) + (int)(this.Stats.Vit * .2f);

            if (this.StatMods != null)
            {
                foreach (TimedStatModifier mod in this.StatMods)
                {
                    switch (mod.AffectedStat)
                    {
                        case TimedStatModifier.Stat.PhysAttack:
                            this.PhysicalAttack += mod.Amount;
                            break;
                        case TimedStatModifier.Stat.PhysDefense:
                            this.PhysicalDefense += mod.Amount;
                            break;
                        case TimedStatModifier.Stat.MagAttack:
                            this.MagicalAttack += mod.Amount;
                            break;
                        case TimedStatModifier.Stat.MagDefense:
                            this.MagicalDefense += mod.Amount;
                            break;
                    }
                }
            }
        }

        public abstract CharacterAction EnemyTurn();

        /// <summary>
        /// Physical Attack
        /// </summary>
        /// <returns></returns>
        protected virtual CharacterAction PhysicalAttackAction()
        {
            CharacterAction action = new CharacterAction();
            action.Description = "Enemy attacks for {0} damage.";
            action.Type = CharacterActionType.Attack;
            action.Value = this.PhysicalAttack;
            return action;
        }

        /// <summary>
        /// MagicalAttack
        /// </summary>
        /// <returns></returns>
        protected CharacterAction AttackSpellAction()
        {
            CharacterAction action = new CharacterAction();
            action.Type = CharacterActionType.Attack;
            action.AttackType = AttackType.Magical;
            SpellDefinition spellDef = SpellDefinitions.GetSpell("Fire");
            this.Stats.MP -= spellDef.MPCost;
            action.Value = spellDef.CalculatePower(this.Stats.Int);
            action.Description = "Enemy casts fire for {0} damage.";
            return action;
        }

        /// <summary>
        /// Magical Heal
        /// </summary>
        /// <returns></returns>
        protected CharacterAction HealAction()
        {
            CharacterAction action = new CharacterAction();
            action.Type = CharacterActionType.Heal;

            SpellDefinition spellDef = SpellDefinitions.GetSpell("Heal");
            action.Value = spellDef.CalculatePower(this.Stats.Int);
            this.Stats.MP -= spellDef.MPCost;
            action.Description = "Enemy heals and recovers {0} HP.";
            return action;
        }

        protected CharacterAction ShieldAction()
        {
            CharacterAction action = new CharacterAction();
            action.Type = CharacterActionType.Shield;
            SpellDefinition def = SpellDefinitions.Spells["Shield"];
            action.Value = def.CalculatePower(this.Stats.Int);
            this.Stats.MP -= def.MPCost;
            action.Description = "Enemy casts shield.";
            return action;
        }

        protected CharacterAction BarrierAction()
        {
            CharacterAction action = new CharacterAction();
            action.Type = CharacterActionType.Barrier;
            SpellDefinition def = SpellDefinitions.Spells["Barrier"];
            action.Value = def.CalculatePower(this.Stats.Int);
            this.Stats.MP -= def.MPCost;
            action.Description = "Enemy casts barrier.";
            return action;
        }

        public void Heal(int amount)
        {
            this.Stats.HP += amount;
            if (this.Stats.HP > this.Stats.MaxHP)
                this.Stats.HP = this.Stats.MaxHP;
        }

        public virtual void DetermineSpells()
        {
            this.availableSpells = null;
        }

        public void AddSpell(string spell)
        {
            if (this.availableSpells != null && !this.availableSpells.Contains(spell))
                this.availableSpells.Add(spell);
        }

        protected virtual bool CanCastSpell(string spellName)
        {
            if (this.availableSpells == null || this.availableSpells.Count == 0)
                return false;

            if (spellName == "Barrier")
            {
                if (this.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.BarrierSpell))
                    return false;
            }
            else if (spellName == "Shield")
            {
                if (this.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.ShieldSpell))
                    return false;
            }

            return this.availableSpells.Contains(spellName) && SpellDefinitions.Spells[spellName].MPCost <= this.Stats.MP;
        }

        public void OnBattleTurnFinished()
        {
            if (this.StatMods != null && this.StatMods.Count > 0)
            {
                this.StatMods.ForEach(x => x.RemainingDuration -= 1);
                this.StatMods.RemoveAll(x => x.RemainingDuration <= 0);
                this.CalculateDerivedStats();
            }
        }
    }
}
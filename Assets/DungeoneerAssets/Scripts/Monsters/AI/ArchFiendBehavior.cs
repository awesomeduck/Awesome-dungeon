using UnityEngine;
using System.Collections;

namespace DungeoneerAssets
{
    public class ArchFiendBehavior : BalancedMonster
    {
        private bool isFocused;
        private bool magicFocused;

        public ArchFiendBehavior()
            : base()
        {
            this.magicFocused = this.MagicalAttack > this.PhysicalAttack;

            if (this.Stats.MaxHP < 100)
                this.Stats.MaxHP = 100;
            if (this.Stats.MaxMP < 99)
                this.Stats.MaxMP = 99;
            this.Stats.HP = this.Stats.MaxHP;
            this.Stats.MP = this.Stats.MaxMP;
        }

        public override void DetermineSpells()
        {
            this.availableSpells = new System.Collections.Generic.List<string>();
            this.availableSpells.Add("Fire");
            this.availableSpells.Add("Shield");
            this.availableSpells.Add("Barrier");
            this.availableSpells.Add("Heal");
        }

        public override CharacterAction EnemyTurn()
        {
            float hpPercent = (float)this.Stats.HP / (float)this.Stats.MaxHP;
            bool canCastHeal = this.CanCastSpell("Heal");
            bool canCastFire = this.CanCastSpell("Fire");

            bool mustHeal = hpPercent < .25f;
            float castCheck = .35f;
            if (!this.isFocused)
            {
                if (mustHeal && canCastHeal)
                    return HealAction();


                float val = UnityEngine.Random.Range(0f, 1f);
                if (val <= castCheck && canCastFire)
                {
                    return AttackSpellAction();
                }
                else if (val <= .70f)
                {
                    return this.PhysicalAttackAction();
                }
                else
                {
                    return this.Focus();
                }
            }
            else
            {
                if (mustHeal && canCastHeal)
                {
                    CharacterAction act = this.HealAction();
                    act.Value = (int)(act.Value * UnityEngine.Random.Range(1.5f, 2f));
                    this.isFocused = false;
                    return act;
                }
                else
                {
                    float val = UnityEngine.Random.Range(0f, 1f);
                    if (val <= castCheck && canCastFire)
                    {
                        CharacterAction act = this.AttackSpellAction();
                        act.Value = (int)(act.Value * UnityEngine.Random.Range(1.5f, 2f));
                        this.isFocused = false;
                        return act;
                    }
                    else
                    {
                        CharacterAction act = this.PhysicalAttackAction();
                        act.Value = (int)(act.Value * UnityEngine.Random.Range(1.5f, 2f));
                        this.isFocused = false;
                        return act;
                    }
                }
            }
        }

        public CharacterAction Focus()
        {
            this.isFocused = true;
            return new CharacterAction
            {
                Type = CharacterActionType.Information,
                Description = "Enemy is focusing it's energy."
            };
        }
    }
}
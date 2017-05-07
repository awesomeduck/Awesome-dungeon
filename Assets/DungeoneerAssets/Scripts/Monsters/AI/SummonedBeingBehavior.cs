using UnityEngine;
using System.Collections;
using System;

namespace DungeoneerAssets
{
    public class SummonedBeingBehavior : GlassCannonMonster
    {
        private bool isFocused;

        public override CharacterAction EnemyTurn()
        {
            float hpPercent = (float)this.Stats.HP / (float)this.Stats.MaxHP;
            bool canCastHeal = this.CanCastSpell("Heal");
            bool canCastFire = this.CanCastSpell("Fire");

            bool mustHeal = hpPercent < .25f;

            if (!this.isFocused)
            {
                if (mustHeal && canCastHeal)
                    return HealAction();
                if (canCastFire)
                {
                    float val = UnityEngine.Random.Range(0f, 1f);
                    if (val <= .65f)
                        return AttackSpellAction();
                    else return this.Focus();
                }
                else return this.PhysicalAttackAction();
            }
            else
            {
                if (mustHeal && canCastHeal)
                {
                    CharacterAction act = this.HealAction();
                    act.Value = (int)(act.Value * UnityEngine.Random.Range(1.3f, 1.5f));
                    this.isFocused = false;
                    return act;
                }
                else
                {
                    if (canCastFire)
                    {
                        CharacterAction act = this.AttackSpellAction();
                        act.Value = (int)(act.Value * UnityEngine.Random.Range(1.3f, 1.5f));
                        this.isFocused = false;
                        return act;
                    }
                    else return this.PhysicalAttackAction();
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
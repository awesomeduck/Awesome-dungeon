using UnityEngine;
using System.Collections;

namespace DungeoneerAssets
{
    public class ChimeraBehavior : FighterMonster
    {
        bool preparingAttack = false;

        public override CharacterAction EnemyTurn()
        {
            if (!this.preparingAttack)
            {
                float val = UnityEngine.Random.Range(0.0f, 1.0f);
                if (val <= .5f)
                    return this.PhysicalAttackAction();
                else if (val <= .75f)
                    return this.BeakStrike();
                else return this.PrepareAttack();
            }
            else
            {
                return this.UnleashPreparedAttack();
            }
        }

        public CharacterAction BeakStrike()
        {
            return new CharacterAction
            {
                AttackType = AttackType.Physical,
                Type = CharacterActionType.Attack,
                Description = "Enemy attacks with Tail Strike for {0} damage",
                Value = (int)(this.PhysicalAttack * UnityEngine.Random.Range(1.1f, 1.3f))
            };
        }

        private CharacterAction UnleashPreparedAttack()
        {
            this.preparingAttack = false;
            return new CharacterAction
            {
                Type = CharacterActionType.Attack,
                AttackType = AttackType.Physical,
                Description = "Enemy attacks wildly for {0} damage.",
                Value = (int)(this.PhysicalAttack * UnityEngine.Random.Range(1.3f, 1.5f))
            };
        }

        public CharacterAction PrepareAttack()
        {
            this.preparingAttack = true;
            return new CharacterAction
            {
                Type = CharacterActionType.Information,
                Description = "Enemy is preparing for a large attack."
            };
        }
    }
}
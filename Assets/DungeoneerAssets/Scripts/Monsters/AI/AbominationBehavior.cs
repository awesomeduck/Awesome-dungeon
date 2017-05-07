using UnityEngine;
using System.Collections;
using System;

namespace DungeoneerAssets
{
    public class AbominationBehavior : TankMonster
    {
        private bool preparingAttack = false;

        public override CharacterAction EnemyTurn()
        {
            if (!this.preparingAttack)
            {
                float val = UnityEngine.Random.Range(0f, 1f);
                if (val <= .65f)
                    return this.PhysicalAttackAction();
                else return this.PrepareAttack();
            }
            else
            {
                return this.UnleashPreparedAttack();
            }
        }

        private CharacterAction UnleashPreparedAttack()
        {
            this.preparingAttack = false;
            return new CharacterAction
            {
                Type = CharacterActionType.Attack,
                AttackType = AttackType.Physical,
                Description = "Enemy charges for {0} damage.",
                Value = (int)(this.PhysicalAttack * UnityEngine.Random.Range(1.5f, 2f))
            };
        }

        public CharacterAction PrepareAttack()
        {
            this.preparingAttack = true;
            return new CharacterAction
            {
                Type = CharacterActionType.Information,
                Description = string.Format("{0} is preparing for a large attack.", this.MonsterName)
            };
        }

    }
}
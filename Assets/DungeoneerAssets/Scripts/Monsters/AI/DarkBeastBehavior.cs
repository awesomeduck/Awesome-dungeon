using UnityEngine;
using System.Collections;

namespace DungeoneerAssets
{
    public class DarkBeastBehavior : FighterMonster
    {
        private bool preparingHaymaker = false;

        public override CharacterAction EnemyTurn()
        {
            if (!preparingHaymaker)
            {
                float val = UnityEngine.Random.Range(0f, 1f);
                if (val <= .5f)
                    return PhysicalAttackAction();
                else if (val <= .8f)
                    return Lunge();
                else return PrepareHaymaker();
            }
            else
            {
                return UnleashHaymaker();
            }
        }

        public CharacterAction Lunge()
        {
            return new CharacterAction
            {
                Type = CharacterActionType.Attack,
                AttackType = AttackType.Physical,
                Description = "Enemy lunges for {0} damage",
                Value = (int)(this.PhysicalAttack * UnityEngine.Random.Range(1.2f, 1.4f))
            };
        }

        public CharacterAction PrepareHaymaker()
        {
            this.preparingHaymaker = true;
            return new CharacterAction
            {
                Type = CharacterActionType.Information,
                Description = "Enemy is preparing a strong attack."
            };
        }

        public CharacterAction UnleashHaymaker()
        {
            this.preparingHaymaker = false;
            return new CharacterAction
            {
                Type = CharacterActionType.Attack,
                AttackType = AttackType.Physical,
                Description = "Haymaker hit for {0}",
                Value = (int)(this.PhysicalAttack * UnityEngine.Random.Range(1.0f, 1.6f))
            };
        }
    }
}
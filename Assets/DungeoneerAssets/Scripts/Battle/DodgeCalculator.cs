using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class DodgeCalculator
    {
        private static float maxDodgeChance = .15f;
        public int PlayerAgility { get; set; }
        public int EnemyAgility { get; set; }

        public float PlayerDodgeChance { get; private set; }
        public float EnemyDodgeChance { get; private set; }

        public bool EnemyDodged { get; private set; }
        public bool PlayerDodged { get; private set; }

        public void CalculateForPlayer()
        {
            this.DetermineOdds();

            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand <= this.PlayerDodgeChance)
                this.PlayerDodged = true;
            else this.PlayerDodged = false;
        }

        public void CalculateForEnemy()
        {
            this.DetermineOdds();
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand <= this.EnemyDodgeChance)
                this.EnemyDodged = true;
            else this.EnemyDodged = false;
        }

        public void DetermineOdds()
        {
            // diff for player attack
            this.EnemyDodgeChance = (float)(this.EnemyAgility - this.PlayerAgility) / (this.EnemyAgility + this.PlayerAgility);
            this.PlayerDodgeChance = (float)(this.PlayerAgility - this.EnemyAgility) / (this.PlayerAgility + this.EnemyAgility);

            if (this.EnemyDodgeChance > maxDodgeChance)
                this.EnemyDodgeChance = maxDodgeChance;

            if (this.PlayerDodgeChance > maxDodgeChance)
                this.PlayerDodgeChance = maxDodgeChance;
        }

        public void Calculate()
        {
            this.DetermineOdds();

            this.CalculateForEnemy();
            this.CalculateForPlayer();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class MonsterStatCalculator
    {
        public double PowerScale { get; set; }
        public MonsterType MonType { get; set; }
        public MonsterBase Monster { get; set; }

        public void Calculate()
        {
            int points = this.DeterminePoints();
            StatWeights weights = MonsterStatWeights.statWeights[MonType];
            for (int i = 0; i < points; i++)
            {
                StatType st = weights.GetStatToIncrement();
                switch (st)
                {
                    case StatType.Str:
                        this.Monster.Stats.Str += 1;
                        break;
                    case StatType.Vit:
                        this.Monster.Stats.Vit += 1;
                        break;
                    case StatType.Int:
                        this.Monster.Stats.Int += 1;
                        break;
                    case StatType.Agi:
                        this.Monster.Stats.Agi += 1;
                        break;
                }
            }

            double factor = UnityEngine.Random.Range(.25f, .50f);
            int maxHP = (int)Math.Pow((factor * this.Monster.Stats.Vit), 2);
            this.Monster.Stats.MaxHP += maxHP;

            factor = UnityEngine.Random.Range(.25f, .50f);
            this.Monster.Stats.MaxMP += (int)Math.Pow((factor * this.Monster.Stats.Int), 2);
            if (this.Monster.Stats.MaxMP > 99)
                this.Monster.Stats.MaxMP = 99;
            this.Monster.Stats.HP = this.Monster.Stats.MaxHP;
            this.Monster.Stats.MP = this.Monster.Stats.MaxMP;
        }


        public int DeterminePoints()
        {
            if (this.PowerScale < 7)
            {
                return (int)((PowerScale * UnityEngine.Random.Range(0.5f, 1f)) * UnityEngine.Random.Range((float)PowerScale, (float)PowerScale + 2));
            }
            else
            {
                return (int)((PowerScale * UnityEngine.Random.Range(.5f, .75f)) * UnityEngine.Random.Range((float)PowerScale, (float)PowerScale + 1));
            }
        }
    }
}
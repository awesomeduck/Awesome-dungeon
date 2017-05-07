using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class StatWeights : List<StatWeight>
    {
        public void CalculatedAdjustedWeights()
        {
            var sorted = this.OrderBy(x => x.Weight).ToList();
            for (int i = 0; i < sorted.Count(); i++)
            {
                if (i == 0)
                    sorted[i].AdjustedWeight = sorted[i].Weight;
                else
                    sorted[i].AdjustedWeight = sorted[i].Weight + sorted[i - 1].AdjustedWeight;
            }
        }

        public StatType GetStatToIncrement()
        {
            float d = UnityEngine.Random.Range(0.0f, 1.0f);
            var item = this.OrderBy(x => x.AdjustedWeight).FirstOrDefault(x => d <= x.AdjustedWeight);
            return item.Type;
        }
    }
}
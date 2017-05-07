using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeoneerAssets
{
    public static class LevelUpDefinitions
    {
        public static readonly Dictionary<int, int> XPPerLevel;
        private static readonly int MaxLevel = 99;

        static LevelUpDefinitions()
        {
            XPPerLevel = new Dictionary<int, int>();

            XPPerLevel.Add(1, 20);

            for (int i = 2; i < MaxLevel; i++)
                XPPerLevel.Add(i, (10 * i * (i + 1)));
        }
    }
}
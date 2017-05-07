using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class ItemDefinition : DefinitionBase
    {
        public Func<int> CalculatedEffect { get; set; }
        public int MaxCapacity { get; set; }
        public Func<Player, bool> CanUse { get; set; }
    }
}
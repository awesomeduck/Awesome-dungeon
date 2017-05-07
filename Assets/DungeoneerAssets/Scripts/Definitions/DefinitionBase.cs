using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public abstract class DefinitionBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool AvailableInStore { get; set; }        

        public int ViewOrder { get; set; }
        public int Cost { get; set; }
        public bool Passive { get; set; }

        public Func<ExplorationData, bool> FloorRequirementMet { get; set; }
    }
}
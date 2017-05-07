using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class FloorExplorationData
    {
        public string FloorName { get; set; }
        public List<MapSpaceInfo> ExploredSpaces { get; set; }
    }
}
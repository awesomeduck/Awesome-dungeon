using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class Interaction
    {
        public Location Location { get; set; }
        public InteractionType Type { get; set; }
    }
}
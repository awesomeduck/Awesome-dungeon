using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class BossCompletion
    {
        public bool Defeated { get; set; }
        public Location Location { get; set; }
    }
}
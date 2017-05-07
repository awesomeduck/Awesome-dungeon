using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class Location
    {
        public string FloorName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            Location l2 = obj as Location;
            if (l2 != null &&
                l2.FloorName == this.FloorName &&
                l2.X == this.X &&
                l2.Y == this.Y)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + FloorName.GetHashCode();
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();
            return hash;
        }
    }
}
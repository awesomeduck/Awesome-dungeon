using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class StatRequirement
    {
        public int Str { get; set; }
        public int Vit { get; set; }
        public int Int { get; set; }
        public int Agi { get; set; }

        public string GetDescription(string separation = " ", bool showEmpties = false)
        {
            string val = "";
            if (showEmpties || this.Str >= 0)
                val += string.Format("Str: {0}{1}", this.Str, separation);
            if (showEmpties || this.Vit >= 0)
                val += string.Format("Vit: {0}{1}", this.Vit, separation);
            if (showEmpties || this.Int >= 0)
                val += string.Format("Int: {0}{1}", this.Int, separation);
            if (showEmpties || this.Agi >= 0)
                val += string.Format("Agi: {0}{1}", this.Agi, separation);

            if (!string.IsNullOrEmpty(val))
                val.Insert(0, "Required ");
            return val;
        }
    }
}
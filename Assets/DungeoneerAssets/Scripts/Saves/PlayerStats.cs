using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    [Serializable]
    public class PlayerStats : Stats
    {

        public int Level { get; set; }
        public int Gold { get; set; }
        public int CurrentXP { get; set; }
        public int Points { get; set; }

        public void Hydrate(Player player)
        {
            this.HP = player.HP;
            this.MP = player.MP;
            this.MaxHP = player.MaxHP;
            this.MaxMP = player.MaxMP;
            this.Level = player.Level;
            this.Gold = player.Gold;
            this.Str = player.Str;
            this.Vit = player.Vit;
            this.Int = player.Int;
            this.Agi = player.Agi;
            this.CurrentXP = player.CurrentXP;
            this.Points = player.Points;
        }
    }
}
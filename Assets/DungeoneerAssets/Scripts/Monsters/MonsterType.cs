using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public enum MonsterType
    {
        Fighter,
        Mage,
        Tank,
        GlassCannon,
        Balanced,
        SpeedDemon
    }
    /*
    Enemy Types stat weight out of 100
        ○ Fighter - focused on attack power. 
            Str	Vit	Int	Agi
            50	30	0	20
        ○ Mage - focused on Int
            Str	Vit	Int	Agi
            0	30	50	20
        ○ Tank - focused on defense
            Str	Vit	Int	Agi
            40	60	0	0
        ○ Glass Cannon
            Str	Vit	Int	Agi
            20	10	60	10
        ○ Balanced
            Str	Vit	Int	Agi
            25	25	25	25
        ○ Speed Demon
            Str	Vit	Int	Agi
            30	20	0	50
    */
}
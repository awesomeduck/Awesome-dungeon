using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class CharacterAction
    {
        public CharacterActionType Type { get; set; }
        public AttackType AttackType { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
    }
}
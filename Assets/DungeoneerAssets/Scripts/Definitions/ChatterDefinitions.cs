using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public static class ChatterDefinitions
    {
        public static List<ChatterDefinition> chatter;

        static ChatterDefinitions()
        {
            chatter = new List<ChatterDefinition>();
            chatter.Add(new ChatterDefinition { MinFloor = null, MaxFloor = "Map2", Text = "Don't forget to check your equipment before leaving for the dungeon." });
            chatter.Add(new ChatterDefinition { MinFloor = null, MaxFloor = "Map2", Text = "Don't be afraid to spend some time practicing on weaker enemies to get stronger." });
            chatter.Add(new ChatterDefinition { MinFloor = null, MaxFloor = "Map2", Text = "If you're having trouble don't forget to stock up on potions and make sure you have better equipment." });
            chatter.Add(new ChatterDefinition { MinFloor = null, MaxFloor = "Map3", Text = "It can be hard to run away from enemies, so don't forget to bring some smoke bombs." });
            chatter.Add(new ChatterDefinition { MinFloor = null, MaxFloor = "Map3", Text = "You can find out more about equipment, items, and spell/skills by touching the name on it's listing." });
            chatter.Add(new ChatterDefinition { MinFloor = "Map2", MaxFloor = "Map4", Text = "Agile enemies can dodge your sword, but they can't move faster than a spell." });
            chatter.Add(new ChatterDefinition { MinFloor = "Map3", MaxFloor = "Map5", Text = "Make sure you check the stores for new items and equipment every so often." });
            chatter.Add(new ChatterDefinition { MinFloor = "Map2", MaxFloor = "Map5", Text = "Barrier magic and defending will help a lot against spell users." });
            chatter.Add(new ChatterDefinition { MinFloor = "Map6", MaxFloor = null, Text = "Don't forget to use defensive skills and spells to protect yourself from strong enemies." });
            chatter.Add(new ChatterDefinition { MinFloor = "Map6", MaxFloor = null, Text = "Scan can be used to see enemies health and mana. Basing your strategy around running down the enemy's MP can work out well." });
            chatter.Add(new ChatterDefinition { MinFloor = "Map8", MaxFloor = null, Text = "I've heard rumors of some powerful equipment being hidden in the depths of that dungeon. Might be worth investigating." });
            chatter.Add(new ChatterDefinition { MinFloor = "Map4", MaxFloor = null, Text = "Based on the geography of the area and what you've reported about each floor, there should be around 10 floors or so." });
        }
    }

    public class ChatterDefinition
    {
        public string MinFloor { get; set; }
        public string MaxFloor { get; set; }
        public string Text { get; set; }
    }
}
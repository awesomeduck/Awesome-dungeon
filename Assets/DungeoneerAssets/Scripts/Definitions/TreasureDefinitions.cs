using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    /// <summary>
    /// On the map, colors that are 255,255,X are treasure chests and use X as the index in the treasures Definition to determine the contents of the treasure. 
    /// </summary>
    public class TreasureDefinitions
    {
        public static Dictionary<byte, TreasureDefinition> Treasures { get; private set; }

        static TreasureDefinitions()
        {
            Treasures = new Dictionary<byte, TreasureDefinition>();

            Treasures.Add(1, new TreasureDefinition
            {
                Name = "Empty",
                Quantity = 0,
                Type = DefinitionType.Trap
            });

            Treasures.Add(2, new TreasureDefinition
            {
                Name = "HealPotion",
                Quantity = 1,
                Type = DefinitionType.Item
            });

            Treasures.Add(3, new TreasureDefinition
            {
                Name = "HealPotion",
                Quantity = 3,
                Type = DefinitionType.Item
            });

            Treasures.Add(4, new TreasureDefinition
            {
                Name = "MPPotion",
                Quantity = 1,
                Type = DefinitionType.Item
            });

            Treasures.Add(5, new TreasureDefinition
            {
                Name = "SmokeBomb",
                Quantity = 1,
                Type = DefinitionType.Item
            });

            Treasures.Add(6, new TreasureDefinition
            {
                Name = "MPPotion",
                Quantity = 3,
                Type = DefinitionType.Item
            });

            Treasures.Add(7, new TreasureDefinition
            {
                Name = "SmokeBomb",
                Quantity = 3,
                Type = DefinitionType.Item
            });

            Treasures.Add(8, new TreasureDefinition
            {
                Name = "Gold",
                Quantity = 25,
                Type = DefinitionType.Gold
            });

            Treasures.Add(9, new TreasureDefinition
            {
                Name = "Gold",
                Quantity = 50,
                Type = DefinitionType.Gold
            });

            Treasures.Add(10, new TreasureDefinition
            {
                Name = "Gold",
                Quantity = 100,
                Type = DefinitionType.Gold
            });

            Treasures.Add(11, new TreasureDefinition
            {
                Name = "Gold",
                Quantity = 150,
                Type = DefinitionType.Gold
            });

            Treasures.Add(12, new TreasureDefinition
            {
                Name = "Gold",
                Quantity = 200,
                Type = DefinitionType.Gold
            });

            Treasures.Add(13, new TreasureDefinition
            {
                Name = "CopperSword",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });

            Treasures.Add(14, new TreasureDefinition
            {
                Name = "CopperShield",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });

            Treasures.Add(15, new TreasureDefinition
            {
                Name = "CopperArmor",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });
            Treasures.Add(16, new TreasureDefinition
            {
                Name = "CopperHelmet",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });

            Treasures.Add(17, new TreasureDefinition
            {
                Name = "SteelSword",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });

            Treasures.Add(18, new TreasureDefinition
            {
                Name = "SteelArmor",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });

            Treasures.Add(19, new TreasureDefinition
            {
                Name = "HealPotion2",
                Quantity = 1,
                Type = DefinitionType.Item
            });

            Treasures.Add(20, new TreasureDefinition
            {
                Name = "MithrilSword",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });

            Treasures.Add(21, new TreasureDefinition
            {
                Name = "MithrilShield",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });

            Treasures.Add(22, new TreasureDefinition
            {
                Name = "MithrilArmor",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });

            Treasures.Add(23, new TreasureDefinition
            {
                Name = "MithrilHelmet",
                Quantity = 1,
                Type = DefinitionType.Equipment
            });
        }
    }
}
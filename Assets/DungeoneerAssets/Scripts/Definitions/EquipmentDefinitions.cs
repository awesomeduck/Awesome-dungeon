using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public static class EquipmentDefinitions
    {
        public static Dictionary<string, EquipmentDefinition> Equipment { get; private set; }

        static EquipmentDefinitions()
        {
            Equipment = new Dictionary<string, EquipmentDefinition>();
            #region Starter

            Equipment.Add("WoodSword", new EquipmentDefinition
            {
                Name = "WoodSword",
                Value = new EquipmentValue(1, 0, 0, 0),
                Description = "Attack + 1",
                Type = EquipmentType.Weapon,
                AvailableInStore = false,
                Cost = 25,
                DisplayName = "Wood Sword",
                ViewOrder = 0
            });

            Equipment.Add("PlainClothes", new EquipmentDefinition
            {
                Name = "PlainClothes",
                DisplayName = "Plain Clothes",
                Value = new EquipmentValue(0, 1, 0, 0),
                Description = "Defense + 1",
                Type = EquipmentType.Armor,
                AvailableInStore = false,
                Cost = 25,
                ViewOrder = 1
            });

            #endregion

            #region Copper

            Equipment.Add("CopperSword", new EquipmentDefinition
            {
                Name = "CopperSword",
                Value = new EquipmentValue(2, 0, 0, 0),
                Description = "Attack + 2",
                Type = EquipmentType.Weapon,
                AvailableInStore = true,
                Cost = 110,
                DisplayName = "Copper Sword",
                ViewOrder = 2,
            });

            Equipment.Add("CopperShield", new EquipmentDefinition
            {
                Name = "CopperShield",
                DisplayName = "Copper Shield",
                Value = new EquipmentValue(0, 2, 0, 0),
                Description = "Defense + 2",
                Type = EquipmentType.Shield,
                AvailableInStore = true,
                Cost = 100,
                ViewOrder = 3
            });

            Equipment.Add("CopperArmor", new EquipmentDefinition
            {
                Name = "CopperArmor",
                DisplayName = "Copper Armor",
                Value = new EquipmentValue(0, 2, 0, 0),
                Description = "Defense + 2",
                Type = EquipmentType.Armor,
                AvailableInStore = true,
                Cost = 120,
                ViewOrder = 4
            });

            Equipment.Add("CopperHelmet", new EquipmentDefinition
            {
                Name = "CopperHelmet",
                DisplayName = "Copper Helmet",
                Value = new EquipmentValue(0, 1, 0, 0),
                Description = "Defense + 1",
                Type = EquipmentType.Helmet,
                AvailableInStore = true,
                Cost = 100,
                ViewOrder = 5
            });
            #endregion

            #region Iron

            Equipment.Add("IronSword", new EquipmentDefinition
            {
                Name = "IronSword",
                DisplayName = "Iron Sword",
                Value = new EquipmentValue(5, 0, 0, 0),
                Description = "Attack + 5",
                Type = EquipmentType.Weapon,
                AvailableInStore = true,
                Cost = 200,
                ViewOrder = 6,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map3"))
            });

            Equipment.Add("IronShield", new EquipmentDefinition
            {
                Name = "IronShield",
                DisplayName = "Iron Shield",
                Value = new EquipmentValue(0, 3, 0, 0),
                Description = "Defense + 3",
                Type = EquipmentType.Shield,
                AvailableInStore = true,
                Cost = 180,
                ViewOrder = 7,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map3"))
            });

            Equipment.Add("IronArmor", new EquipmentDefinition
            {
                Name = "IronArmor",
                DisplayName = "Iron Armor",
                Value = new EquipmentValue(0, 3, 0, 0),
                Description = "Defense + 3",
                Type = EquipmentType.Armor,
                AvailableInStore = true,
                Cost = 180,
                ViewOrder = 8,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map3"))
            });

            Equipment.Add("IronHelmet", new EquipmentDefinition
            {
                Name = "IronHelmet",
                DisplayName = "Iron Helmet",
                Value = new EquipmentValue(0, 2, 0, 0),
                Description = "Defense + 2",
                Type = EquipmentType.Helmet,
                AvailableInStore = true,
                Cost = 150,
                ViewOrder = 9,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map3"))
            });

            #endregion

            #region Steel

            Equipment.Add("SteelSword", new EquipmentDefinition
            {
                Name = "SteelSword",
                DisplayName = "Steel Sword",
                Value = new EquipmentValue(8, 0, 0, 0),
                Description = "Attack + 8",
                Type = EquipmentType.Weapon,
                AvailableInStore = true,
                Cost = 600,
                ViewOrder = 10,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map5"))
            });

            Equipment.Add("SteelShield", new EquipmentDefinition
            {
                Name = "SteelShield",
                DisplayName = "Steel Shield",
                Value = new EquipmentValue(0, 5, 0, 0),
                Description = "Defense + 5",
                Type = EquipmentType.Shield,
                AvailableInStore = true,
                Cost = 400,
                ViewOrder = 11,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map5"))
            });

            Equipment.Add("SteelArmor", new EquipmentDefinition
            {
                Name = "SteelArmor",
                DisplayName = "Steel Armor",
                Value = new EquipmentValue(0, 5, 0, 0),
                Description = "Defense + 5",
                Type = EquipmentType.Armor,
                AvailableInStore = true,
                Cost = 450,
                ViewOrder = 12,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map5"))
            });

            Equipment.Add("SteelHelmet", new EquipmentDefinition
            {
                Name = "SteelHelmet",
                DisplayName = "Steel Helmet",
                Value = new EquipmentValue(0, 3, 0, 0),
                Description = "Defense + 3",
                Type = EquipmentType.Helmet,
                AvailableInStore = true,
                Cost = 380,
                ViewOrder = 13,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map5"))
            });
            #endregion

            #region Mithril            
            Equipment.Add("MithrilSword", new EquipmentDefinition
            {
                Name = "MithrilSword",
                DisplayName = "Mithril Sword",
                Value = new EquipmentValue(12, 0, 4, 0),
                Description = "Phys Attack + 12, Mag. Attack + 4",
                Type = EquipmentType.Weapon,
                AvailableInStore = false,
                Cost = 0,
                ViewOrder = 10,
            });

            Equipment.Add("MithrilShield", new EquipmentDefinition
            {
                Name = "MithrilShield",
                DisplayName = "Mithril Shield",
                Value = new EquipmentValue(0, 7, 0, 2),
                Description = "Phys Defense + 7, Mag. Defense + 2",
                Type = EquipmentType.Shield,
                AvailableInStore = false,
                Cost = 0,
                ViewOrder = 11,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map9"))
            });

            Equipment.Add("MithrilArmor", new EquipmentDefinition
            {
                Name = "MithrilArmor",
                DisplayName = "Mithril Armor",
                Value = new EquipmentValue(0, 8, 0, 4),
                Description = "Phys Defense + 8, Mag. Defense + 4",
                Type = EquipmentType.Armor,
                AvailableInStore = false,
                Cost = 0,
                ViewOrder = 12,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map8"))
            });

            Equipment.Add("MithrilHelmet", new EquipmentDefinition
            {
                Name = "MithrilHelmet",
                DisplayName = "Mithril Helmet",
                Value = new EquipmentValue(0, 6, 0, 1),
                Description = "Phys Defense + 6, Mag. Defense + 1",
                Type = EquipmentType.Helmet,
                AvailableInStore = false,
                Cost = 0,
                ViewOrder = 13,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map9"))
            });
            #endregion
        }

        public static EquipmentDefinition Get(string name)
        {
            if (Equipment.ContainsKey(name))
                return Equipment[name];

            return null;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DungeoneerAssets
{
    public class ItemDefinitions
    {
        public static Dictionary<string, ItemDefinition> Items { get; private set; }

        static ItemDefinitions()
        {
            Items = new Dictionary<string, ItemDefinition>();
            Items.Add("HealPotion", new ItemDefinition
            {
                Name = "HealPotion",
                DisplayName = "Heal Potion",
                Description = "Restores a small amount of health.",
                CalculatedEffect = () => { return 15; },
                ViewOrder = 0,
                Cost = 20,
                AvailableInStore = true,
                MaxCapacity = 9,
                CanUse = (player) =>
                    {
                        if (player.HP >= player.MaxHP)
                            return false;
                        return true;
                    }
            });

            Items.Add("SmokeBomb", new ItemDefinition
            {
                Name = "SmokeBomb",
                DisplayName = "Smoke Bomb",
                Description = "Immediately escape from a fight.",
                CalculatedEffect = () =>
                    {
                        return 1;
                    },
                ViewOrder = 2,
                AvailableInStore = true,
                MaxCapacity = 3,
                Cost = 30,
                CanUse = (player) =>
                    {
                        return BattleManager.InBattle && !BattleManager.InBossBattle;
                    }
            });

            Items.Add("MPPotion", new ItemDefinition
            {
                Name = "MPPotion",
                DisplayName = "MP Potion",
                Description = "Restores a small amount of mana.",
                CalculatedEffect = () => { return 20; },
                ViewOrder = 1,
                Cost = 30,
                MaxCapacity = 9,
                AvailableInStore = true,
                CanUse = (player) =>
                    {
                        if (player.MP >= player.MaxMP)
                            return false;
                        return true;
                    }
            });

            Items.Add("WarpCrystal", new ItemDefinition
            {
                Name = "WarpCrystal",
                DisplayName = "Warp Crystal",
                Description = "Allows fast travel to specific floors. Can only be used in town.",
                CalculatedEffect = () => { return 1; },
                ViewOrder = 3,
                Cost = 500,
                MaxCapacity = 1,
                AvailableInStore = true,
                CanUse = (player) =>
                    {
                        return false;
                    },
                Passive = true,
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map3"))
            });

            Items.Add("ExitCrystal", new ItemDefinition
            {
                Name = "ExitCrystal",
                DisplayName = "Exit Crystal",
                Description = "Quickly return to town, breaks upon use.",
                CalculatedEffect = () => { return 1; },
                ViewOrder = 3,
                Cost = 50,
                MaxCapacity = 1,
                AvailableInStore = true,
                CanUse = (player) =>
                {
                    return !FloorManager.InTown && !BattleManager.InBattle;
                },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map2"))
            });

            Items.Add("HealPotion2", new ItemDefinition
            {
                Name = "HealPotion2",
                DisplayName = "Heal Potion X",
                Description = "Restores a large amount of health.",
                CalculatedEffect = () => { return 50; },
                ViewOrder = 0,
                Cost = 200,
                AvailableInStore = true,
                MaxCapacity = 9,
                CanUse = (player) =>
                {
                    if (player.HP >= player.MaxHP)
                        return false;
                    return true;
                },
                FloorRequirementMet = new Func<ExplorationData, bool>(x => x.ExploredAreas.ContainsKey("Map6"))
            });

            Items.Add("MPPotionX", new ItemDefinition
            {
                Name = "MPPotionX",
                DisplayName = "MP Potion X",
                Description = "Restores a large amount of mana.",
                CalculatedEffect = () => { return 60; },
                ViewOrder = 1,
                Cost = 500,
                MaxCapacity = 9,
                AvailableInStore = true,
                CanUse = (player) =>
                    {
                        if (player.MP >= player.MaxMP)
                            return false;
                        return true;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(x => x.ExploredAreas.ContainsKey("Map6"))
            });
        }
    }
}
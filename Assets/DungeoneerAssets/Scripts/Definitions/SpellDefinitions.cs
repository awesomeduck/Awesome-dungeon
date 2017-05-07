using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DungeoneerAssets
{
    public static class SpellDefinitions
    {
        public static Dictionary<string, SpellDefinition> Spells { get; private set; }

        static SpellDefinitions()
        {
            Spells = new Dictionary<string, SpellDefinition>();
            Spells.Add("Fire", new SpellDefinition
            {
                Name = "Fire",
                DisplayName = "Fire",
                Description = "Attack the enemy with fire.",
                MPCost = 6,
                AttackType = AttackType.Magical,
                StatRequirement = new StatRequirement { Int = 6 },
                CalculatePower = (playerInt) =>
                {
                    int basePower = 4;
                    if (playerInt < 5)
                        return basePower + playerInt;
                    else return basePower + 5 + (playerInt / 2);
                },
                ViewOrder = 1,
                AvailableInStore = true,
                Cost = 150,
                CanUse = (player) =>
                    {
                        return BattleManager.InBattle;
                    }
            });

            Spells.Add("Heal", new SpellDefinition
            {
                Name = "Heal",
                DisplayName = "Heal",
                MPCost = 6,
                AttackType = AttackType.Magical,
                Description = "Restore some of the player's health.",
                CalculatePower = (playerInt) =>
                    {
                        int basePower = 8;
                        if (playerInt < 5)
                            return basePower + (playerInt * 2);
                        else return basePower + 10 + (playerInt);
                    },
                ViewOrder = 0,
                AvailableInStore = true,
                Cost = 100,
                CanUse = (player) =>
                    {
                        return player.HP < player.MaxHP;
                    }
            });

            // show enemy hp and stats
            Spells.Add("Scan", new SpellDefinition
            {
                Name = "Scan",
                DisplayName = "Scan",
                MPCost = 2,
                AttackType = AttackType.Magical,
                Description = "Reveals the enemy's health.",
                CalculatePower = (playerInt) =>
                {
                    return 1;
                },
                ViewOrder = 1,
                AvailableInStore = true,
                Cost = 100,
                CanUse = (player) =>
                {
                    return BattleManager.InBattle;
                }
            });

            // phys def +
            Spells.Add("Shield", new SpellDefinition
            {
                Name = "Shield",
                DisplayName = "Shield",
                MPCost = 8,
                AttackType = AttackType.Magical,
                Description = "Improves physical defense for a short time.",
                StatRequirement = new StatRequirement { Int = 6 },
                CalculatePower = (playerInt) =>
                {
                    return (int)(playerInt * .5f);
                },
                ViewOrder = 2,
                AvailableInStore = true,
                Cost = 350,
                CanUse = (player) =>
                {
                    return BattleManager.InBattle;
                }
            });

            // mg def+
            Spells.Add("Barrier", new SpellDefinition
            {
                Name = "Barrier",
                DisplayName = "Barrier",
                Description = "Improves magical defense for a short time.",
                MPCost = 8,
                AttackType = AttackType.Magical,
                StatRequirement = new StatRequirement { Int = 6 },
                CalculatePower = (playerInt) =>
                {
                    return (int)(playerInt * .5f);
                },
                ViewOrder = 2,
                AvailableInStore = true,
                Cost = 350,
                CanUse = (player) =>
                {
                    return BattleManager.InBattle;
                }
            });

            Spells.Add("Exit", new SpellDefinition
            {
                Name = "Exit",
                DisplayName = "Exit",
                Description = "Instantly return to town.",
                MPCost = 10,
                AttackType = AttackType.Magical,
                CalculatePower = (playerInt) => { return 1; },
                ViewOrder = 10,
                AvailableInStore = true,
                Cost = 400,
                CanUse = player =>
                    {
                        return !BattleManager.InBattle && !FloorManager.InTown;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map2"))
            });

            Spells.Add("Focus", new SpellDefinition
            {
                Name = "Focus",
                DisplayName = "Focus",
                Description = "Next spell cast will be 1.5x as powerful.",
                MPCost = 8,
                AttackType = AttackType.Magical,
                StatRequirement = new StatRequirement { Int = 6 },
                CalculatePower = (playerInt) => { return 1; },
                ViewOrder = 3,
                AvailableInStore = true,
                Cost = 500,
                CanUse = player =>
                    {
                        return BattleManager.InBattle && !player.IsFocused;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map3"))
            });

            Spells.Add("Meditate", new SpellDefinition
            {
                Name = "Meditate",
                DisplayName = "Meditate",
                Description = "Restore a small amount of MP.",
                MPCost = 6,
                AttackType = AttackType.Magical,
                StatRequirement = new StatRequirement { Int = 8 },
                CalculatePower = (playerInt) =>
                {
                    return 0;
                },
                ViewOrder = 3,
                AvailableInStore = true,
                Cost = 500,
                CanUse = player =>
                    {
                        return BattleManager.InBattle && player.MP < player.MaxMP;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map3"))
            });

            Spells.Add("FierceAttack", new SpellDefinition
            {
                Name = "FierceAttack",
                DisplayName = "Fierce Attack",
                Description = "Higher damage physical attack.",
                MPCost = 7,
                AttackType = AttackType.Physical,
                StatRequirement = new StatRequirement { Str = 8 },
                CalculatePower = (playerInt) =>
                {
                    return (int)(playerInt * 1.5f);
                },
                ViewOrder = 4,
                AvailableInStore = true,
                Cost = 350,
                CanUse = player =>
                    {
                        return BattleManager.InBattle;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map2"))
            });

            Spells.Add("Stun", new SpellDefinition
            {
                Name = "Stun",
                DisplayName = "Stun",
                Description = "Attack with the chance to stun the enemy.",
                MPCost = 5,
                AttackType = AttackType.Physical,
                StatRequirement = new StatRequirement { Str = 6 },
                CalculatePower = (playerInt) =>
                {
                    return UnityEngine.Random.Range(0.0f, 1.0f) >= 0.65f ? 0 : 1; // 65% chance of stunning
                },
                ViewOrder = 4,
                AvailableInStore = true,
                Cost = 200,
                CanUse = player =>
                    {
                        return BattleManager.InBattle;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map2"))
            });

            Spells.Add("DoubleAttack", new SpellDefinition
            {
                Name = "DoubleAttack",
                DisplayName = "Double Attack",
                Description = "Attack twice in the same turn.",
                MPCost = 7,
                AttackType = AttackType.Physical,
                StatRequirement = new StatRequirement { Agi = 6 },
                CalculatePower = (playerInt) =>
                {
                    return 1;
                },
                ViewOrder = 5,
                AvailableInStore = true,
                Cost = 350,
                CanUse = player =>
                    {
                        return BattleManager.InBattle;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map4"))
            });

            Spells.Add("ExtendedGuard", new SpellDefinition
            {
                Name = "ExtendedGuard",
                DisplayName = "Extended Guard",
                Description = "Less damage taken for the next 3 turns.",
                MPCost = 8,
                AttackType = AttackType.Physical,
                StatRequirement = new StatRequirement { Vit = 6 },
                CalculatePower = (playerInt) =>
                {
                    return 1;
                },
                ViewOrder = 6,
                AvailableInStore = true,
                Cost = 500,
                CanUse = player =>
                    {
                        return BattleManager.InBattle;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map4"))
            });

            Spells.Add("PerfectGuard", new SpellDefinition
            {
                Name = "PerfectGuard",
                DisplayName = "Perfect Guard",
                Description = "Take no damage for the next 2 turns.",
                MPCost = 10,
                AttackType = AttackType.Physical,
                StatRequirement = new StatRequirement { Vit = 8 },
                CalculatePower = (playerInt) =>
                {
                    return 1;
                },
                ViewOrder = 6,
                AvailableInStore = true,
                Cost = 650,
                CanUse = player =>
                    {
                        return BattleManager.InBattle;
                    },
                FloorRequirementMet = new Func<ExplorationData, bool>(ex => ex.ExploredAreas.ContainsKey("Map5"))
            });
        }

        public static SpellDefinition GetSpell(string name)
        {
            if (Spells.ContainsKey(name))
                return Spells[name];

            throw new InvalidOperationException("spell " + name + " isn't defined");
        }
    }
}
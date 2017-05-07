using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class ItemManager : MonoBehaviour
    {
        public EffectManager fxManager;
        public AudioManager audioManager;
        public Player player;
        public BattleManager battleManager;
        public FloorManager floorManager;
        public UIManager uiManager;

        public bool CanUseItem(string itemName, out string reason)
        {
            reason = "";
            if (!this.player.CanUseItem(itemName))
            {
                reason = "You don't need to use this item right now.";
                return false;
            }
            return true;
        }

        public int UseItem(string itemName)
        {
            switch (itemName)
            {
                case "HealPotion":
                case "HealPotion2":
                    this.audioManager.PlaySFX(SFX.Potion);
                    int healAmount = player.UseItem(itemName);
                    this.player.Heal(healAmount);
                    return healAmount;

                case "SmokeBomb":
                    this.fxManager.ShowEffect(Effect.SmokeBomb);
                    break;

                case "MPPotion":
                case "MPPotionX":
                    this.audioManager.PlaySFX(SFX.Potion);
                    int mpAmount = player.UseItem(itemName);
                    this.player.RestoreMP(mpAmount);
                    return mpAmount;

                case "ExitCrystal":
                    player.UseItem(itemName);
                    this.fxManager.ShowEffect(Effect.ExitSpell);
                    this.audioManager.PlaySFX(SFX.ExitSpell);
                    this.uiManager.DeactivateAll();
                    floorManager.Invoke("GoToTown", this.fxManager.exitSpell.GetComponent<ParticleSystem>().duration);
                    break;
            }

            return 0;
        }

        public string GetDescription(string itemName, int amount)
        {
            switch (itemName)
            {
                case "HealPotion":
                case "HealPotion2":
                    return string.Format("player healed for {0} hp.", amount);

                case "SmokeBomb":
                    break;

                case "MPPotion":
                case "MPPotionX":
                    return string.Format("player restored {0} mp.", amount);

                case "ExitCrystal":
                    return "Returning to town";
            }
            return null;
        }
    }
}
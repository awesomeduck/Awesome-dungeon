using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class PlayerEquipmentMenu : ShopBase<EquipmentSlot>
    {
        public BattleManager battleManager;
        public AudioManager audioManager;
        public bool Sell { get; set; }

        protected override void ConfirmationRequestedHandler(object obj)
        {
            EquipmentSlot slot = (EquipmentSlot)obj;
            if (this.Sell)
            {
                if (slot.Definition.Cost == 0)
                {
                    this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                    this.messageText.text = "I can't buy something like that.";
                    return;
                }
                if (slot.Definition.Cost <= 500)
                    this.audioManager.PlaySFX(SFX.CheapPurchase);
                else
                    this.audioManager.PlaySFX(SFX.ExpensivePurchase);

                if (slot.IsEquipped)
                    this.player.CurrentInventory.UnEquipType(slot.Definition.Type);
                this.player.Gold += slot.Definition.Cost;
                this.player.CurrentInventory.Equipment.Remove(slot);
                this.content.Remove(slot);
                this.messageText.text = string.Format("{0} sold for {1}g.", slot.Definition.DisplayName, slot.Definition.Cost);

            }
            else
            {
                if (!slot.Definition.StatRequirementsMet(this.player))
                {
                    this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                    this.messageText.text = "You don't meet the stat requirements to equip that.";
                    return;
                }

                this.audioManager.PlaySFX(SFX.Equip);
                this.player.CurrentInventory.EquipItem(slot);
            }
            this.LoadPage();
            this.player.CalculateBattleStats();
        }

        protected override void DescriptionRequestedHandler(object def)
        {
            EquipmentSlot slot = (EquipmentSlot)def;
            this.messageText.text = slot.Definition.Description;
            if (slot.Definition.StatRequirements != null)
            {
                this.messageText.text += Environment.NewLine;
                this.messageText.text += "Requires - ";
                if (slot.Definition.StatRequirements.Str > 0)
                    this.messageText.text += "Str: " + slot.Definition.StatRequirements.Str + " ";

                if (slot.Definition.StatRequirements.Vit > 0)
                    this.messageText.text += "Vit: " + slot.Definition.StatRequirements.Vit + " ";

                if (slot.Definition.StatRequirements.Int > 0)
                    this.messageText.text += "Int: " + slot.Definition.StatRequirements.Int + " ";

                if (slot.Definition.StatRequirements.Agi > 0)
                    this.messageText.text += "Agi: " + slot.Definition.StatRequirements.Agi;
            }
        }

        protected override void LoadContent()
        {
            this.content = this.player.CurrentInventory.Equipment
                .OrderBy(x => x.Definition.ViewOrder)
                .ThenBy(x => x.Name)
                .ToList();
        }

        protected override string GetCost(EquipmentSlot item)
        {
            if (this.Sell)
                return item.Definition.Cost.ToString();
            else
            {
                if (item.Definition.Type == EquipmentType.Weapon)
                    return item.Definition.Value.PhysicalAttack.ToString();
                else return item.Definition.Value.PhysicalDefense.ToString();
            }
        }

        protected override string GetDisplayName(EquipmentSlot item)
        {
            if (item.IsEquipped)
                return "(E) " + item.Definition.DisplayName;
            else return item.Definition.DisplayName;
        }

        protected override Color GetTextColor(EquipmentSlot item)
        {
            if (item.IsEquipped)
                return Color.yellow;
            else return base.GetTextColor(item);
        }
        protected override string GetName(EquipmentSlot item)
        {
            return item.Name;
        }

        protected override string GetActionText(EquipmentSlot def)
        {
            if (this.Sell)
                return "Sell";
            else
            {
                if (!def.Definition.StatRequirementsMet(this.player))
                    return null;
                else if (!def.IsEquipped)
                    return "Equip";
                else return null;
            }
        }
    }
}
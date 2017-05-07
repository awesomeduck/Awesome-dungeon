using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class PlayerItemMenu : ShopBase<ItemSlot>
    {
        public BattleManager battleManager;
        public AudioManager audioManager;
        public bool Sell { get; set; }
        public ItemManager itemManager;

        protected override string GetActionText(ItemSlot def)
        {
            string reason = "";
            if (this.Sell)
                return "Sell";
            else if (def.Definition.Passive)
                return null;
            else if (!itemManager.CanUseItem(def.Definition.Name, out reason))
                return null;
            else
                return "Use";
        }

        protected override Color GetTextColor(ItemSlot item)
        {
            string reason = "";
            if (this.Sell)
                return base.GetTextColor(item);
            if (item.Definition.Passive)
                return Color.grey;
            else if (!itemManager.CanUseItem(item.Definition.Name, out reason))
                return Color.grey;
            else return base.GetTextColor(item);
        }

        protected override string GetCost(ItemSlot item)
        {
            if (this.Sell)
                return item.Definition.Cost.ToString();
            else return item.Quantity.ToString();
        }

        protected override string GetDisplayName(ItemSlot item)
        {
            if (this.Sell)
                return item.Definition.DisplayName + " x " + item.Quantity;
            else
                return item.Definition.DisplayName;
        }

        protected override void LoadContent()
        {
            this.content = this.player.CurrentInventory.Items
                .OrderBy(x => x.Definition.ViewOrder)
                .ThenBy(x => x.Definition.Name)
                .Cast<ItemSlot>()
                .ToList();
        }

        protected override string GetName(ItemSlot item)
        {
            return item.Name;
        }

        protected override void ConfirmationRequestedHandler(object obj)
        {
            ItemSlot slot = (ItemSlot)obj;
            if (this.Sell)
            {
                if (slot.Definition.Cost <= 500)
                    this.audioManager.PlaySFX(SFX.CheapPurchase);
                else
                    this.audioManager.PlaySFX(SFX.ExpensivePurchase);
                this.player.Gold += slot.Definition.Cost;
                slot.Quantity -= 1;
                this.messageText.text = string.Format("{0} sold for {1}g.", slot.Definition.DisplayName, slot.Definition.Cost);
                if (slot.Quantity == 0)
                {
                    this.player.CurrentInventory.Items.Remove(slot);
                    this.content.Remove(slot);
                }
            }
            else
            {
                string reason;
                if (!this.itemManager.CanUseItem(slot.Name, out reason))
                {
                    this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                    this.messageText.text = reason;
                    return;
                }

                if (BattleManager.InBattle)
                    this.battleManager.PlayerItem(slot.Definition.Name);
                else this.UseItem(slot);

            }

            this.LoadContent();
            this.LoadPage();
        }

        private void UseItem(ItemSlot slot)
        {
            int amount = this.itemManager.UseItem(slot.Name);
            this.messageText.text = this.itemManager.GetDescription(slot.Name, amount);
        }

        protected override void DescriptionRequestedHandler(object obj)
        {
            ItemSlot slot = (ItemSlot)obj;
            this.messageText.text = slot.Definition.Description;
        }
    }
}
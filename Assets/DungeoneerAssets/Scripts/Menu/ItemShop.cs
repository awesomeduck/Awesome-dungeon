using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class ItemShop : ShopBase<ItemDefinition>
    {
        public AudioManager audioManager;

        protected override string GetCost(ItemDefinition def) { return def.Cost.ToString(); }
        protected override string GetName(ItemDefinition def) { return def.Name; }
        protected override string GetDisplayName(ItemDefinition def) { return def.DisplayName; }

        protected override void LoadContent()
        {
            this.content = ItemDefinitions.Items.Values.Where(x => x.AvailableInStore && (x.FloorRequirementMet == null || x.FloorRequirementMet(this.player.ExplorationInfo)))
                .OrderBy(x => x.ViewOrder)
                .ThenBy(x => x.Name)
                .Cast<ItemDefinition>()
                .ToList();
        }

        protected override void ConfirmationRequestedHandler(object content)
        {
            ItemDefinition def = (ItemDefinition)content;
            if (this.player.Gold < def.Cost)
            {
                this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                this.messageText.text = "You can't afford that.";
            }
            else if (this.player.CurrentInventory.Items.Any(x => x.Definition == def) && this.player.CurrentInventory.Items.First(x => x.Definition == def).Quantity >= def.MaxCapacity)
            {
                this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                this.messageText.text = "You can't hold more of those.";
            }
            else
            {
                if (def.Cost <= 500)
                    this.audioManager.PlaySFX(SFX.CheapPurchase);
                else
                    this.audioManager.PlaySFX(SFX.ExpensivePurchase);
                this.player.Gold -= def.Cost;
                this.player.CurrentInventory.AddItem((ItemDefinition)def, 1);
                this.messageText.text = def.DisplayName + " purchased.";
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class EquipmentShop : ShopBase<EquipmentDefinition>
    {
        public AudioManager audioManager;

        protected override string GetCost(EquipmentDefinition def) { return def.Cost.ToString(); }
        protected override string GetName(EquipmentDefinition def) { return def.Name; }
        protected override string GetDisplayName(EquipmentDefinition def) { return def.DisplayName; }

        protected override void LoadContent()
        {
            this.content = EquipmentDefinitions.Equipment.Values.Where(x => x.AvailableInStore
                && (x.FloorRequirementMet == null || x.FloorRequirementMet(this.player.ExplorationInfo)))
                .OrderBy(x => x.ViewOrder)
                .ThenBy(x => x.Name)
                .Cast<EquipmentDefinition>()
                .ToList();
        }

        protected override void ConfirmationRequestedHandler(object obj)
        {
            EquipmentDefinition def = (EquipmentDefinition)obj;
            if (this.player.Gold < def.Cost)
            {
                this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                this.messageText.text = "You can't afford that.";
            }
            else if (this.player.CurrentInventory.Equipment.Any(x => x.Definition == def))
            {
                this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                this.messageText.text = "You already own that.";
            }
            else
            {
                if (def.Cost <= 500)
                    this.audioManager.PlaySFX(SFX.CheapPurchase);
                else
                    this.audioManager.PlaySFX(SFX.ExpensivePurchase);
                this.player.Gold -= def.Cost;
                this.player.CurrentInventory.AddEquipment((EquipmentDefinition)def);
                this.messageText.text = def.DisplayName + " purchased. Don't forget to check your equipment before heading out.";
            }
        }

        protected override void DescriptionRequestedHandler(object def)
        {
            base.DescriptionRequestedHandler(def);

            EquipmentDefinition eDef = (EquipmentDefinition)def;
            if (eDef != null && eDef.StatRequirements != null)
            {
                this.messageText.text += Environment.NewLine;
                this.messageText.text += "Requires - ";
                if (eDef.StatRequirements.Str > 0)
                    this.messageText.text += "Str: " + eDef.StatRequirements.Str + " ";

                if (eDef.StatRequirements.Vit > 0)
                    this.messageText.text += "Vit: " + eDef.StatRequirements.Vit + " ";

                if (eDef.StatRequirements.Int > 0)
                    this.messageText.text += "Int: " + eDef.StatRequirements.Int + " ";

                if (eDef.StatRequirements.Agi > 0)
                    this.messageText.text += "Agi: " + eDef.StatRequirements.Agi;
            }
        }

    }
}
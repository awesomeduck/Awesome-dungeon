using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class SpellShop : ShopBase<SpellDefinition>
    {
        public AudioManager audioManager;

        protected override string GetCost(SpellDefinition def) { return def.Cost.ToString(); }
        protected override string GetName(SpellDefinition def) { return def.Name; }
        protected override string GetDisplayName(SpellDefinition def) { return def.DisplayName; }

        protected override void LoadContent()
        {
            this.content = SpellDefinitions.Spells.Values.Where(x => x.AvailableInStore && (x.FloorRequirementMet == null || x.FloorRequirementMet(this.player.ExplorationInfo)))
                .Except(this.player.Spells)
                .OrderBy(x => x.ViewOrder)
                .ThenBy(x => x.Name)
                .Cast<SpellDefinition>()
                .ToList();
        }

        protected override void ConfirmationRequestedHandler(object def)
        {
            SpellDefinition spellDef = (SpellDefinition)def;
            if (this.player.Spells.Contains(spellDef))
            {
                this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                messageText.text = "You already own that.";
            }
            else if (this.player.Gold < spellDef.Cost)
            {
                this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                messageText.text = "You can't afford that.";
            }
            else
            {
                if (spellDef.Cost <= 500)
                    this.audioManager.PlaySFX(SFX.CheapPurchase);
                else
                    this.audioManager.PlaySFX(SFX.ExpensivePurchase);
                this.player.Gold -= spellDef.Cost;
                this.player.Spells.Add(spellDef);
                messageText.text = spellDef.DisplayName + " purchased.";
                this.RemoveItem((SpellDefinition)def);
            }
        }

        protected override void DescriptionRequestedHandler(object def)
        {
            SpellDefinition spellDef = def as SpellDefinition;
            if (spellDef != null)
            {
                this.messageText.text = spellDef.Description;
                if (spellDef.StatRequirement != null)
                {
                    string req = spellDef.StatRequirement.GetDescription();
                    if (!string.IsNullOrEmpty(req))
                    {
                        this.messageText.text += Environment.NewLine;
                        this.messageText.text += req;
                    }
                }
            }
        }
    }
}
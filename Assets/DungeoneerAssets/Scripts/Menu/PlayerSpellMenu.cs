using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class PlayerSpellMenu : ShopBase<SpellDefinition>
    {
        public BattleManager battleManager;
        public SpellManager spellManager;
        public AudioManager audioManager;

        protected override void LoadContent()
        {
            this.content = this.player.Spells
                .OrderBy(x => x.ViewOrder)
                .ThenBy(x => x.Name)
                .ToList();
        }

        protected override void ConfirmationRequestedHandler(object obj)
        {
            SpellDefinition def = (SpellDefinition)obj;
            string reason;
            if (!this.spellManager.CanCastSpell(def, out reason))
            {
                this.audioManager.PlaySFX(SFX.ButtonNegativeClick);
                this.messageText.text = reason;
                return;
            }

            if (BattleManager.InBattle)
                this.battleManager.PlayerSpell(((SpellDefinition)obj).Name);
            else
                this.CastSpell(def);
        }

        private void CastSpell(SpellDefinition def)
        {
            int amount = this.spellManager.CastSpell(def.Name);
            if (def.Name == "Heal") // special case cause I suck
                player.Heal(amount);
            this.messageText.text = this.spellManager.GetDescriptionText(def.Name, amount);
        }

        protected override void DescriptionRequestedHandler(object def)
        {
            SpellDefinition spellDef = (SpellDefinition)def;
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

        protected override string GetCost(SpellDefinition item)
        {
            return item.MPCost.ToString();
        }

        protected override string GetActionText(SpellDefinition def)
        {
            string reason = "";
            if (!spellManager.CanCastSpell(def, out reason))
                return null;
            else return "Cast";
        }

        protected override string GetDisplayName(SpellDefinition item)
        {
            return item.DisplayName;
        }

        protected override string GetName(SpellDefinition item)
        {
            return item.Name;
        }

        protected override Color GetTextColor(SpellDefinition item)
        {
            string reason = "";
            if (!spellManager.CanCastSpell(item, out reason))
                return Color.grey;
            else
                return base.GetTextColor(item);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class TreasureChest : MonoBehaviour, IInteractable
    {
        private static readonly string description = "There's a chest ahead. Will you open it?";
        private static readonly string yesButtonText = "Open";

        public DefinitionBase Contents { get; set; }
        public int Quantity { get; set; }

        public Material OpenMaterial;
        public bool IsOpened { get; set; }
        public Location Location { get; set; }
        public AudioManager audioManager { get; set; }

        string IInteractable.Description
        {
            get { return description; }
        }

        string IInteractable.YesButtonText
        {
            get { return yesButtonText; }
        }

        string IInteractable.NoButtonText
        {
            get { return null; }
        }

        public string YesFinishedText
        {
            get
            {
                if (this.Contents == null)
                    return "There was nothing.";
                else
                {
                    if (this.Contents.Name == "Gold")
                    {
                        return string.Format("You found {0} gold.", this.Quantity);
                    }
                    else
                    {
                        if (this.Quantity > 1)
                            return string.Format("You found {0} x {1}", this.Contents.DisplayName, this.Quantity);
                        else
                        {
                            if (this.Contents.DisplayName.StartsWithVowel())
                                return string.Format("You found an {0}", this.Contents.DisplayName);
                            else return string.Format("You found a {0}", this.Contents.DisplayName);
                        }
                    }
                }
            }
        }

        public string NoFinishedText
        {
            get { return null; }
        }

        public bool IsRepeatable { get { return false; } }

        public void OnYes(InteractionArgs args)
        {
            this.SetIsOpened();
            this.audioManager.PlaySFX(SFX.ChestOpen);
            if (this.Contents != null)
            {
                if (this.Contents is EquipmentDefinition)
                {
                    args.Player.CurrentInventory.AddEquipment((EquipmentDefinition)this.Contents);
                }
                else if (this.Contents is ItemDefinition)
                {
                    if (this.Contents.Name == "Gold")
                        args.Player.Gold += this.Quantity;
                    else
                        args.Player.CurrentInventory.AddItem((ItemDefinition)this.Contents, this.Quantity);
                }
                else if (this.Contents is SpellDefinition)
                {
                    if (!args.Player.Spells.Contains((SpellDefinition)this.Contents))
                        args.Player.Spells.Add((SpellDefinition)this.Contents);
                }
            }

            args.Player.FinishedInteractions.Add(new Interaction { Location = this.Location, Type = InteractionType.Treasure });
        }

        public void SetIsOpened()
        {
            this.IsOpened = true;
            // change material to open. 
            this.gameObject.GetComponent<Renderer>().material = this.OpenMaterial;

        }

        public void OnNo(InteractionArgs args)
        {
            // not showing the button. 
        }
    }
}
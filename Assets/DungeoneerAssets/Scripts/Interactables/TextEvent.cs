using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class TextEvent : MonoBehaviour, IInteractable
    {                    
        public string Description { get; set; }
        public string YesButtonText { get; set; }
        public string NoButtonText { get { return null; } }
        public string NoFinishedText { get { return null; } }        
        public string YesFinishedText { get { return null; } }
        public string[] TextLines { get; set; }
        public Location Location { get; set; }
        public bool IsSignPost { get; set; }
        public bool IsRepeatable { get { return true; } }

        public bool DoNotShow { get; internal set; }
        public bool ViewOnce { get; internal set; }
        public bool Teleport { get; set; }
        public Location TeleportTarget { get; set; }

        public void OnNo(InteractionArgs p)
        {
            // not an option. 
        }

        private IEnumerator ShowText(Player p)
        {            
            PlayerNavigation playerNav = p.GetComponent<PlayerNavigation>();
            if (playerNav != null)
                playerNav.enabled = false;
            UIManager.Instance.textEventBox.ShowMessage(this.TextLines);
            while (UIManager.Instance.textEventBox.gameObject.activeInHierarchy)
                yield return null;
            p.FinishedInteractions.Add(new Interaction { Location = this.Location, Type = InteractionType.TextEvent });
            if (this.Teleport)
                this.PerformTeleport(p);
            if (playerNav != null)
                playerNav.enabled = true;            
        }

        private void PerformTeleport(Player p)
        {
            p.transform.position = new Vector3(this.TeleportTarget.X, p.transform.position.y, this.TeleportTarget.Y);
            
        }

        public void OnYes(InteractionArgs p)
        {            
            this.StartCoroutine(this.ShowText(p.Player));            
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!this.DoNotShow)
            {           
                if (!this.IsSignPost)
                {
                    if (!string.IsNullOrEmpty(this.Description))
                    {
                        UIManager.Instance.interactionPanel.gameObject.SetActive(true);
                        UIManager.Instance.interactionPanel.GetComponent<InteractionPanel>().UpdateContent(this);
                    }
                    else
                    {
                        Player p = col.GetComponent<Player>();
                        if (p != null)
                        {
                            this.StartCoroutine(this.ShowText(p));
                            if (this.ViewOnce)
                                this.DoNotShow = true;
                        }
                    }
                }
                else 
                {
                    // because raycast triggers while we're inside the trigger collider I want to disable this while the player can't actually see the signpost.
                    this.DoNotShow = true;
                    this.Invoke("ResetDoNotShow", .1f);
                }
            }
        }

        private void ResetDoNotShow()
        {
            this.DoNotShow = false;
        }

        private void OnTriggerExit(Collider col)
        {
            if(this.IsSignPost && this.DoNotShow)
            {
                Player p = col.GetComponent<Player>();
                if(p != null)
                {
                    // restore so the player can read the signpost again.
                    this.DoNotShow = false;
                }
            }

            if (!this.IsSignPost && !string.IsNullOrEmpty(this.Description))
            {
                UIManager.Instance.interactionPanel.gameObject.SetActive(false);                
            }
        }
    }
}
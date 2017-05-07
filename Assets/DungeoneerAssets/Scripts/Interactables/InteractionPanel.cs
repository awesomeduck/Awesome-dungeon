using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class InteractionPanel : MonoBehaviour
    {
        public Text txtControl;
        public Button btnYes;
        public Button btnNo;
        public Player player;
        private IInteractable currentInteractable;

        public void YesClicked()
        {
            this.StartCoroutine("OnYesClicked");
        }

        public void NoClicked()
        {
            this.StartCoroutine("OnNoClicked");
        }

        private void OnEnable()
        {
            BattleManager.AllowBattle = false;
        }

        private void OnDisable()
        {
            BattleManager.AllowBattle = true;
        }

        private IEnumerator OnYesClicked()
        {
            if (!this.currentInteractable.IsRepeatable)
            {
                this.btnYes.interactable = false;
                this.btnNo.interactable = false;
                this.txtControl.text = this.currentInteractable.YesFinishedText;
            }
            this.currentInteractable.OnYes(new InteractionArgs { Player = player });            
            yield return null;
        }

        private IEnumerator OnNoClicked()
        {
            if (!this.currentInteractable.IsRepeatable)
            {
                this.btnYes.interactable = false;
                this.btnNo.interactable = false;
                this.txtControl.text = this.currentInteractable.NoFinishedText;
            }
            this.currentInteractable.OnNo(new InteractionArgs { Player = player });
            
            yield return new WaitForSeconds(2.0f);
            this.ClearContent();
            this.gameObject.SetActive(false);
        }

        private void ClearContent()
        {
            this.currentInteractable = null;
            this.txtControl.text = "";
            this.btnYes.UpdateText("");
            this.btnNo.UpdateText("");
        }

        internal void UpdateContent(IInteractable interactable)
        {
            if (interactable != null)
            {
                this.currentInteractable = interactable;
                txtControl.text = interactable.Description;
                if (!string.IsNullOrEmpty(interactable.YesButtonText))
                {
                    btnYes.gameObject.SetActive(true);
                    btnYes.interactable = true;
                    btnYes.UpdateText(interactable.YesButtonText);
                }
                else
                    btnYes.gameObject.SetActive(false);

                if (!string.IsNullOrEmpty(interactable.NoButtonText))
                {
                    btnNo.gameObject.SetActive(true);
                    btnNo.interactable = true;
                    btnNo.UpdateText(interactable.NoButtonText);
                }
                else
                    btnNo.gameObject.SetActive(false);
            }
        }
    }
}
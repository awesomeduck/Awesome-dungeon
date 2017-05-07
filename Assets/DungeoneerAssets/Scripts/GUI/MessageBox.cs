using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class MessageBox : MonoBehaviour
    {
        public Text txtControl;
        public Button btnYes;
        public Button btnNo;

        private Action onNo;
        private Action onYes;

        public void YesClicked()
        {
            this.btnYes.interactable = false;
            this.btnNo.interactable = false;
            this.gameObject.SetActive(false);
            Action act = this.onYes;
            this.Clear();
            if (act != null)
                act();

        }

        public void NoClicked()
        {
            this.btnYes.interactable = false;
            this.btnNo.interactable = false;
            this.gameObject.SetActive(false);
            Action act = this.onNo;
            this.Clear();
            if (act != null)
                act();
        }

        public void Clear()
        {
            this.txtControl.text = "";
            this.btnYes.UpdateText("");
            this.btnNo.UpdateText("");
            this.onNo = null;
            this.onYes = null;
        }

        public void Show(string text, string confirmText, string declineText,
            Action yesAction, Action noAction)
        {
            this.gameObject.SetActive(true);
            this.txtControl.text = text;
            if (onYes != null || !string.IsNullOrEmpty(confirmText))
            {
                this.btnYes.gameObject.SetActive(true);
                this.onYes = yesAction;
                this.btnYes.interactable = true;
                this.btnYes.UpdateText(confirmText);
            }

            if (onNo != null || !string.IsNullOrEmpty(declineText))
            {
                this.btnNo.gameObject.SetActive(true);
                this.btnNo.interactable = true;
                this.onNo = noAction;
                this.btnNo.UpdateText(declineText);
            }
            else this.btnNo.gameObject.SetActive(false);
        }

    }
}

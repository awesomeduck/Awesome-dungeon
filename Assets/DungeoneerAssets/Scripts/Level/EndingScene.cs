using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class EndingScene : MonoBehaviour
    {
        public string[] text;
        private int curIndex = 0;
        public Text txtDisplay;
        public GameObject btnNextText;
        public EffectManager fxManager;
        public UIManager uiManager;
        public Player player;

        public void OnEnable()
        {
            btnNextText.SetActive(false);
            this.InvokeRepeating("NextText", 3f, 4f);
        }

        public void NextText()
        {
            if (this.curIndex < this.text.Length)
                txtDisplay.text = text[curIndex];
            curIndex++;
            if (curIndex > 3)
            {
                btnNextText.SetActive(false);
                this.StartCoroutine("TeleportStart");
                this.CancelInvoke("NextText");
            }
        }

        private IEnumerator TeleportStart()
        {
            yield return new WaitForSeconds(2f);
            fxManager.ShowEffect(Effect.ExitSpell);
            yield return new WaitForSeconds(.5f);
            txtDisplay.text = "Huh, guess we're heading right into the next adventure.";
            yield return new WaitForSeconds(4f);
            uiManager.OverrideState(UIManager.UIState.CreditsNoBack);
        }
    }
}
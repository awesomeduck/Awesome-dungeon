using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class Inn : MonoBehaviour
    {
        public Player player;
        public int Cost;
        public Text messageText;
        public AudioManager audioManager;
        public GameObject btnChatter;
        private IEnumerable<string> chatter;
        private int currentChatterIndex;


        private void OnEnable()
        {
            this.messageText.text = string.Format("It will cost {0} gold to stay the night.", this.Cost);
            this.currentChatterIndex = 0;
            this.chatter = ChatterDefinitions.chatter.Where(x => (x.MinFloor == null || player.ExplorationInfo.ExploredAreas.Keys.Contains(x.MinFloor)) &&
                (x.MaxFloor == null || !player.ExplorationInfo.ExploredAreas.Keys.Contains(x.MaxFloor)))
                .Select(x => x.Text);
            if (!this.chatter.Any())
                this.btnChatter.SetActive(false);
            else this.btnChatter.SetActive(true);
        }

        public void Sleep()
        {
            if (this.player.Gold < this.Cost)
                this.messageText.text = "You can't afford that.";
            else if (this.player.HP == this.player.MaxHP && this.player.MP == this.player.MaxMP)
                this.messageText.text = "You're already at full power...";
            else
            {
                this.audioManager.PlaySFX(SFX.CheapPurchase);
                this.player.Gold -= this.Cost;
                this.player.RestoreToFull();
            }
        }

        public void DisplayChatter()
        {
            if (this.currentChatterIndex >= this.chatter.Count())
                this.currentChatterIndex = 0;

            this.messageText.text = this.chatter.ElementAt(this.currentChatterIndex);
            this.currentChatterIndex++;
        }
    }
}
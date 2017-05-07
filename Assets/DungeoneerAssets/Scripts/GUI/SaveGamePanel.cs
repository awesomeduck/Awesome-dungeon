using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.ComponentModel;
using System.Text;

namespace DungeoneerAssets
{
    public class SaveGamePanel : MonoBehaviour
    {
        public event Action<SaveGameState> Clicked;
        private SaveGameState saveState;

        public Text txtSaveDate;
        public Text txtLocation;
        public Text txtPlayTime;
        public Text txtLevel;
        public Text txtHP;
        public Text txtMP;
        public Text txtXP;
        public Text txtGold;

        public SaveGameState SaveState
        {
            get { return this.saveState; }
            set { if (this.saveState != value) { this.saveState = value; this.UpdateDisplay(); } }
        }

        public void ClickHandler()
        {
            this.OnClicked(this.saveState);
        }

        protected void OnClicked(SaveGameState state)
        {
            Action<SaveGameState> handler = this.Clicked;
            if (handler != null)
                handler(state);
        }

        private void UpdateDisplay()
        {
            if (this.saveState != null)
            {
                txtSaveDate.text = this.saveState.SaveDate.ToString();
                txtLocation.text = this.saveState.CurrentFloorName;
                TimeSpan ts = this.saveState.PlayTime;
                StringBuilder sb = new StringBuilder();
                if (ts.Days > 0)
                    sb.AppendFormat("{0} days ", ts.Days);
                sb.AppendFormat("{0}h {1}m {2}s", ts.Hours, ts.Minutes, ts.Seconds);

                txtPlayTime.text = sb.ToString();
                txtLevel.text = string.Format("Level: {0}", this.saveState.PlayerStats.Level);
                txtHP.text = string.Format("HP: {0} / {1}", this.saveState.PlayerStats.HP, this.saveState.PlayerStats.MaxHP);
                txtMP.text = string.Format("MP: {0} / {1}", this.saveState.PlayerStats.MP, this.saveState.PlayerStats.MaxMP);
                txtXP.text = string.Format("XP: {0} / {1}", this.saveState.PlayerStats.CurrentXP, LevelUpDefinitions.XPPerLevel[this.saveState.PlayerStats.Level]);
                txtGold.text = string.Format("Gold: {0}", this.saveState.PlayerStats.Gold);
            }
        }

        public void ClearDisplay()
        {
            txtSaveDate.text = "";
            txtLocation.text = "";
            txtPlayTime.text = "";
            txtLevel.text = "";
            txtHP.text = "";
            txtMP.text = "";
            txtXP.text = "";
            txtGold.text = "";
        }
    }
}
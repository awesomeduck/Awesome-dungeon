using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class PlayerStatus : MonoBehaviour
    {
        private int startStr;
        private int startVit;
        private int startAgi;
        private int startInt;
        private int startPoints;

        public Player player;
        public GameObject[] statButtons;

        public Text txtLevel;
        public Text txtHP;
        public Text txtMP;
        public Text txtXP;
        public Text txtNext;
        public Text txtGold;
        public Text txtStr;
        public Text txtVit;
        public Text txtAgi;
        public Text txtInt;
        public Text txtPoints;

        public Text txtPAtk;
        public Text txtMAtk;
        public Text txtPDef;
        public Text txtMDef;

        public GameObject btnBack;
        public GameObject btnAccept;
        public GameObject btnCancel;

        public Text messagePanel;
        public GameObject statButtonContainer;

        private void OnEnable()
        {
            this.EnableStatButtons(this.player.Points > 0);
            this.startStr = this.player.Str;
            this.startVit = this.player.Vit;
            this.startAgi = this.player.Agi;
            this.startInt = this.player.Int;
            this.startPoints = this.player.Points;

            if (this.player.Points > 0)
            {
                this.btnBack.SetActive(false);
                this.btnAccept.SetActive(true);
                this.btnCancel.SetActive(true);
            }
            else
            {
                this.btnBack.SetActive(true);
                this.btnAccept.SetActive(false);
                this.btnCancel.SetActive(false);
            }

            this.Refresh();
            this.UpdateCalculatedStats();
        }

        private void Refresh()
        {
            txtLevel.text = this.player.Level.ToString();
            txtHP.text = string.Format("{0} / {1}", this.player.HP, this.player.MaxHP);
            txtMP.text = string.Format("{0} / {1}", this.player.MP, this.player.MaxMP);
            txtXP.text = this.player.CurrentXP.ToString();
            txtNext.text = this.player.XPToNextLevel.ToString();
            txtGold.text = this.player.Gold.ToString();
            txtStr.text = this.player.Str.ToString();
            txtVit.text = this.player.Vit.ToString();
            txtAgi.text = this.player.Agi.ToString();
            txtInt.text = this.player.Int.ToString();
            txtPoints.text = this.player.Points.ToString();
        }

        private void EnableStatButtons(bool active)
        {
            statButtonContainer.SetActive(active);
        }

        public void AddStat(string stat)
        {
            if (this.player.Points > 0)
            {
                switch (stat)
                {
                    case "Str":
                        this.player.Str += 1;
                        this.player.Points -= 1;
                        this.txtStr.text = this.player.Str.ToString();
                        this.txtPoints.text = this.player.Points.ToString();
                        break;
                    case "Vit":
                        this.player.Vit += 1;
                        this.player.Points -= 1;
                        this.txtVit.text = this.player.Vit.ToString();
                        this.txtPoints.text = this.player.Points.ToString();
                        break;
                    case "Agi":
                        this.player.Agi += 1;
                        this.player.Points -= 1;
                        this.txtAgi.text = this.player.Agi.ToString();
                        this.txtPoints.text = this.player.Points.ToString();
                        break;
                    case "Int":
                        this.player.Int += 1;
                        this.player.Points -= 1;
                        this.txtInt.text = this.player.Int.ToString();
                        this.txtPoints.text = this.player.Points.ToString();
                        break;
                }
                this.UpdateCalculatedStats();
            }
        }

        public void DecStat(string stat)
        {
            if (this.startPoints > 0)
            {
                switch (stat)
                {
                    case "Str":
                        if (this.startStr < this.player.Str)
                        {
                            this.player.Str -= 1;
                            this.player.Points += 1;
                            this.txtStr.text = this.player.Str.ToString();
                            this.txtPoints.text = this.player.Points.ToString();
                        }
                        break;
                    case "Vit":
                        if (this.startVit < this.player.Vit)
                        {
                            this.player.Vit -= 1;
                            this.player.Points += 1;
                            this.txtVit.text = this.player.Vit.ToString();
                            this.txtPoints.text = this.player.Points.ToString();
                        }
                        break;
                    case "Agi":
                        if (this.startAgi < this.player.Agi)
                        {
                            this.player.Agi -= 1;
                            this.player.Points += 1;
                            this.txtAgi.text = this.player.Agi.ToString();
                            this.txtPoints.text = this.player.Points.ToString();
                        }
                        break;
                    case "Int":
                        if (this.startInt < this.player.Int)
                        {
                            this.player.Int -= 1;
                            this.player.Points += 1;
                            this.txtInt.text = this.player.Int.ToString();
                            this.txtPoints.text = this.player.Points.ToString();
                        }
                        break;
                }
                this.UpdateCalculatedStats();
            }
        }

        private void UpdateCalculatedStats()
        {
            this.player.CalculateBattleStats();
            this.txtMAtk.text = this.player.MagicalAttack.ToString();
            this.txtMDef.text = this.player.MagicalDefense.ToString();
            this.txtPAtk.text = this.player.PhysicalAttack.ToString();
            this.txtPDef.text = this.player.PhysicalDefense.ToString();
        }

        public void Cancel()
        {
            this.player.Points = this.startPoints;
            this.player.Str = this.startAgi;
            this.player.Int = this.startInt;
            this.player.Vit = this.startVit;
            this.player.Agi = this.startAgi;
            this.player.CalculateBattleStats();
        }

        public void SetDescriptionText(string type)
        {
            if (!string.IsNullOrEmpty(type))
            {
                switch (type)
                {
                    case "Level":
                        messagePanel.text = "Level - The overall measure of your power.";
                        break;
                    case "HP":
                        messagePanel.text = "Health Points - If you lose all of them you're dead.";
                        break;
                    case "MP":
                        messagePanel.text = "Mana Points - Used to cast spells and use combat skills.";
                        break;
                    case "XP":
                        messagePanel.text = "Experience Points - The amount of progress you've made towards your next level-up.";
                        break;
                    case "NextXP":
                        messagePanel.text = "Next Level - The amount of experience needed to reach the next level.";
                        break;
                    case "Gold":
                        messagePanel.text = "Gold - Money to be spent at stores in town.";
                        break;
                    case "Str":
                        messagePanel.text = "Strength - Your physical power.";
                        break;
                    case "Vit":
                        messagePanel.text = "Vitality - Affects your durability and resistance to attacks.";
                        break;
                    case "Int":
                        messagePanel.text = "Intelligence - your capacity for learning and casting spells.";
                        break;
                    case "Agi":
                        messagePanel.text = "Agility - your speed and nimbleness in the face of danger.";
                        break;
                    case "Points":
                        messagePanel.text = "Points - Spend to improve your stats; earned when you level-up.";
                        break;
                    case "PAtk":
                        messagePanel.text = "Physical Attack - Rating of your basic attack with a weapon.";
                        break;
                    case "PDef":
                        messagePanel.text = "Physical Defense - Rating of your resistance to physical attacks.";
                        break;
                    case "MAtk":
                        messagePanel.text = "Magical Attack - rating of your spell damage.";
                        break;
                    case "MDef":
                        messagePanel.text = "Magical Defense - rating of your resistance to magical attacks.";
                        break;
                }
            }
        }
    }
}
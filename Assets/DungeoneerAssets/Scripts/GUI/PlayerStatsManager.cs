using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class PlayerStatsManager : MonoBehaviour
    {
        public UnityEngine.UI.Text playerHP;
        public UnityEngine.UI.Text playerMP;
        public UnityEngine.UI.Text playerLevel;
        public Text playerGold;
        public Image pointsImg;

        public Player player;

        private void Start()
        {
            this.player.PropertyChanged += player_PropertyChanged;
            this.UpdateAllText();
        }

        void player_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Level":
                    playerLevel.text = player.Level.ToString();
                    break;
                case "HP":
                case "MaxHP":
                    int hp = this.player.HP;
                    if (hp < 0)
                        hp = 0;
                    this.playerHP.text = string.Format("{0} / {1}", hp, this.player.MaxHP);
                    break;
                case "MP":
                case "MaxMP":
                    this.playerMP.text = string.Format("{0} / {1}", this.player.MP, this.player.MaxMP);
                    break;
                case "Gold":
                    this.playerGold.text = this.player.Gold.ToString();
                    break;
                case "Points":
                    this.pointsImg.gameObject.SetActive(this.player.Points > 0);
                    break;
            }
        }

        private void UpdateAllText()
        {
            this.playerLevel.text = this.player.Level.ToString();
            int hp = this.player.HP;
            if (hp < 0)
                hp = 0;
            this.playerHP.text = string.Format("{0} / {1}", hp, this.player.MaxHP);
            this.playerMP.text = string.Format("{0} / {1}", this.player.MP, this.player.MaxMP);
            this.playerGold.text = this.player.Gold.ToString();
            this.pointsImg.gameObject.SetActive(this.player.Points > 0);
        }


        private void OnDestroy()
        {
            this.player.PropertyChanged -= player_PropertyChanged;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class GameOverPanel : MonoBehaviour
    {
        public Button btnReturnToTown;
        public Button btnLoadGame;
        public Player player;
        public SaveGameComponent saveGameComponent;

        private void OnEnable()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Return to Town");
            sb.AppendFormat("({0}g)", (int)(this.player.Gold / 4));
            btnReturnToTown.UpdateText(sb.ToString());

            if (this.saveGameComponent.LastLoadedIndex != -1)
                btnLoadGame.gameObject.SetActive(true);
            else btnLoadGame.gameObject.SetActive(false);
        }
    }
}
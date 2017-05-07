using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class DungeonEntrance : MonoBehaviour
    {
        public Player player;
        public FloorManager floorManager;
        public UIManager uiManager;

        public void GoToDungeon()
        {
            if (this.player.CurrentInventory.Items.Any(x => x.Name == "WarpCrystal") && this.player.ExplorationInfo.ExploredAreas.Count > 0)
            {
                this.uiManager.PushState(UIManager.UIState.DungeonFloorChoice);
            }
            else
            {
                this.floorManager.GoToFloor("Map1");
            }
        }
    }
}
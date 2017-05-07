using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class TitleScreen : MonoBehaviour
    {
        public GameObject btnResume;
        public UIManager uiManager;
        public MessageBox msgBox;
        public SaveGameComponent saveGameComponent;
        public GameObject btnLoad;
        public GameObject btnSave;
        public bool GameInProgress;
        public Player player;
        public BattleManager battleManager;
        public FloorManager floorManager;

        private void OnEnable()
        {
            SaveGameManager sm = new SaveGameManager();
            bool hasSaves = sm.HasSaveFiles();
            if (hasSaves)
                btnLoad.SetActive(true);
            else btnLoad.SetActive(false);

            if (BattleManager.InBattle)
                this.btnSave.SetActive(false);
            else if (SaveGameComponent.GameInProgress)
                btnSave.SetActive(true);
        }

        public void EnableResume(bool val)
        {
            this.btnResume.SetActive(val);
            this.GameInProgress = true;
        }

        public void OnNewGame()
        {
            this.saveGameComponent.GameStartDate = DateTime.Now;
            this.saveGameComponent.PlayTime = null;
            this.saveGameComponent.LastLoadedDate = null;
            this.battleManager.Reset();
            this.uiManager.DeactivateAll();
            this.player.DefaultInitialize();
            this.floorManager.BossCompletion.Clear();
            this.uiManager.OverrideState(UIManager.UIState.OpeningScene);
            this.btnSave.SetActive(true);
            SaveGameComponent.GameInProgress = true;
        }

        public void OnExit()
        {
            msgBox.Show("Are you sure you want to exit?", "Yes", "No",
                () =>
                {
                    if (this.GameInProgress)
                    {
                        if (!BattleManager.InBattle)
                        {
                            msgBox.Show("Would you like to save your progress before quitting?",
                                "Save", "Don't Save",
                                () =>
                                {
                                    if (this.saveGameComponent.LastLoadedIndex != -1)
                                        this.saveGameComponent.Save(this.saveGameComponent.LastLoadedIndex);
                                    else
                                    {
                                        this.saveGameComponent.SaveNew();
                                    }
                                    Application.Quit();
                                },
                                () => Application.Quit());
                        }
                        else
                        {
                            msgBox.Show("You can't save while you're in battle. Exit anyway?",
                                "Exit", "Cancel",
                                () => Application.Quit(), null);
                        }
                    }
                    else
                        Application.Quit();
                }, null);
        }
    }
}
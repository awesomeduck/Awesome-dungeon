using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class SaveGameComponent : MonoBehaviour
    {
        public Player player;
        public FloorManager floorManager;
        public AudioManager audioManager;
        public BattleManager battleManager;
        public UIManager uiManager;
        private SaveGameManager saveGameManager;
        public DateTime GameStartDate { get; set; }
        public int LastLoadedIndex { get; set; }
        public DateTime? LastLoadedDate { get; set; }
        public TimeSpan? PlayTime { get; set; }
        public static bool GameInProgress;

        private void Start()
        {
            this.saveGameManager = new SaveGameManager();
            this.LastLoadedIndex = -1;
            GameInProgress = false;
        }

        public void Save(int index)
        {
            SaveGameState state = new SaveGameState();
            state.SaveDate = DateTime.Now;
            if (this.PlayTime.HasValue && this.LastLoadedDate.HasValue)
            {
                // append time
                TimeSpan curPlayTime = DateTime.Now - this.LastLoadedDate.Value;
                state.PlayTime = this.PlayTime.Value + curPlayTime;
            }
            else if (!this.LastLoadedDate.HasValue)
            {
                // first save
                state.PlayTime = DateTime.Now - this.GameStartDate;
            }

            state.GameStartedDate = this.GameStartDate;
            state.PlayerStats = new PlayerStats();
            state.PlayerStats.Hydrate(this.player);
            state.PlayerInventory = this.player.CurrentInventory;
            state.PlayerSpells = this.player.Spells.Select(x => x.Name).ToList();
            state.Position = this.player.transform.position;
            state.Direction = this.player.transform.forward;
            state.CurrentFloorName = this.floorManager.floorName;
            state.FinishedInteractions = this.player.FinishedInteractions;
            state.BossCompletions = this.floorManager.BossCompletion;
            state.ExplorationData = new List<FloorExplorationData>();
            foreach (var kvp in this.player.ExplorationInfo.ExploredAreas)
            {
                FloorExplorationData floorExp = new FloorExplorationData();
                floorExp.FloorName = kvp.Key;
                if (kvp.Value != null && kvp.Value.Count > 0)
                {
                    floorExp.ExploredSpaces = new List<MapSpaceInfo>();
                    kvp.Value.ForEach(x => floorExp.ExploredSpaces.Add(x));
                }
                state.ExplorationData.Add(floorExp);
            }
            this.saveGameManager.Save(index, state);
            this.LastLoadedIndex = index;
        }

        public void Load(int index)
        {
            SaveGameState state = this.saveGameManager.Load(index);
            if (state != null)
            {
                this.battleManager.Reset();
                this.uiManager.DeactivateAll();
                this.audioManager.StopBGM();
                this.audioManager.StopSFX();
                this.player.HydrateStats(state.PlayerStats);
                this.player.CurrentInventory = state.PlayerInventory;
                this.player.CurrentInventory.CalibrateDefinitions();
                this.player.HydrateSpells(state.PlayerSpells);
                this.player.FinishedInteractions = state.FinishedInteractions;
                this.player.CalculateBattleStats();
                this.floorManager.BossCompletion = state.BossCompletions;
                this.floorManager.GoToFloor(state.CurrentFloorName);
                this.player.transform.position = state.Position;
                this.player.transform.forward = state.Direction;
                this.floorManager.UpdateInteractableDirection();
                this.GameStartDate = state.GameStartedDate;
                this.LastLoadedIndex = index;
                this.LastLoadedDate = DateTime.Now;
                this.PlayTime = state.PlayTime;
                if (state.ExplorationData != null && state.ExplorationData.Count > 0)
                {
                    this.player.ExplorationInfo = new ExplorationData();
                    foreach (FloorExplorationData floorExp in state.ExplorationData)
                        this.player.ExplorationInfo.ExploredAreas.Add(floorExp.FloorName, floorExp.ExploredSpaces);
                }

                GameInProgress = true;
            }
        }

        internal void SaveNew()
        {
            int index = 0;
            List<SaveGameState> saves = this.saveGameManager.GetAllSaves();
            if (saves != null && saves.Count > 0)
                index = saves.Max(x => x.Index) + 1;

            this.Save(index);
        }

        public void LoadPrevious()
        {
            this.Load(this.LastLoadedIndex);
        }
    }
}
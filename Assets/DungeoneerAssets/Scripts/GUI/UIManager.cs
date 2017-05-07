using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DungeoneerAssets
{
    public class UIManager : MonoBehaviour
    {

        #region Singleton
        private static UIManager instance;
        public static UIManager Instance { get { return instance; } }

        void Awake()
        {
            instance = this;
        }
        #endregion
        public enum UIState
        {
            None,
            Town,
            Dungeon,
            Battle,
            Menu,
            LevelUp,
            Items,
            Status,
            Equipment,
            Spells,
            ItemsSell,
            EquipmentSell,
            ItemShop,
            EquipmentShop,
            SpellShop,
            Inn,
            Bank,
            Map,
            Title,
            Settings,
            Credits,
            DungeonFloorChoice,
            SaveMenu,
            LoadMenu,
            OpeningScene,
            EndingScene,
            CreditsNoBack,
			Glossary,
        }

        private UIState previousState;
        private UIState currentState;
        private Stack<UIState> stateStack = new Stack<UIState>();

        public GameObject playerStats;
        public GameObject navControls;
        public GameObject battleCommands;
        public GameObject gameOver;

        public GameObject dungeonMenu;
        public GameObject mapButton;
        public GameObject enemyName;
        public GameObject playerSpells;
        public GameObject playerItems;
        public GameObject playerStatus;
        public GameObject playerEquipment;
        public GameObject playerMap;

        public GameObject townOutsidePanel;
        public GameObject equipmentShop;
        public GameObject spellShop;
        public GameObject itemShop;
        public GameObject inn;
        public GameObject bank;

        public GameObject titleScreen;
        public GameObject settingsScreen;
        public GameObject creditsScreen;
		public GameObject GlossaryScreen;
        public GameObject dungeonChoiceScreen;

        public GameObject saveMenuScreen;

        public GameObject[] shopSellItems;

        public GameObject interactionPanel;
        public bool MapOpened { get; set; }
        public MessageBox msgBox;
        public GameObject openingScene;
        public GameObject endingScene;
        public GameObject battleIndicatorPanel;
        public TextEventBox textEventBox; 

        private void Start()
        {

            this.StartCoroutine(this.OnApplicationStarted());
        }

        private IEnumerator OnApplicationStarted()
        {
            yield return new WaitForEndOfFrame();
            this.OverrideState(UIState.Title);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {

                if (this.currentState != UIState.Title && this.stateStack.Count == 1)
                {
                    this.PushState(UIState.Title);
                    this.titleScreen.GetComponent<TitleScreen>().EnableResume(true);
                }
                else
                {
                    while (this.stateStack.Count > 1)
                        this.PopState();
                }
            }
        }

        public void OpenGameMenu()
        {
            if (this.currentState != UIState.Title && this.stateStack.Count == 1)
            {
                this.PushState(UIState.Title);
                this.titleScreen.GetComponent<TitleScreen>().EnableResume(true);
            }
        }


        public void PushState(string state)
        {
            UIState newState = (UIState)Enum.Parse(typeof(UIState), state);
            this.PushState(newState);
        }

        public void PushState(UIState state)
        {
            if (this.currentState != state)
            {
                this.stateStack.Push(this.currentState);
                this.UpdateUIState(state);
            }
        }

        public void OverrideState(string state)
        {
            UIState newState = (UIState)Enum.Parse(typeof(UIState), state);
            this.OverrideState(newState);
        }

        public void OverrideState(UIState state)
        {
            this.stateStack.Clear();
            this.stateStack.Push(state);
            this.UpdateUIState(state);
        }

        public void PopState()
        {
            if (this.stateStack.Count > 0)
            {

                if (this.currentState == UIState.Map)
                    this.MapOpened = false;
                UIState state = this.stateStack.Pop();
                this.UpdateUIState(state);
            }
            else
            {
                this.UpdateUIState(UIState.None);
            }
        }

        private void UpdateUIState(UIState state)
        {
            this.previousState = this.currentState;
            this.currentState = state;
            this.LoadCurrentState();
        }

        private void LoadCurrentState()
        {
            this.DeactivateAll();

            switch (this.currentState)
            {
                case UIState.Town:
                    this.townOutsidePanel.SetActive(true);
                    this.ActivateMenuButtons(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Dungeon:
                    this.ActivateMenuButtons(true);
                    this.mapButton.SetActive(true);
                    this.navControls.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Battle:
                    this.battleCommands.SetActive(true);
                    this.enemyName.SetActive(true);
                    this.playerStats.SetActive(true);
                    this.battleIndicatorPanel.SetActive(true);
                    if (BattleManager.IsGameOver)
                    {
                        this.battleCommands.SetActive(false);
                        this.gameOver.SetActive(true);
                        this.battleIndicatorPanel.SetActive(false);
                    }
                    break;
                case UIState.Menu:
                    this.dungeonMenu.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.LevelUp:
                case UIState.Status:
                    this.playerStatus.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Items:
                    this.PlayerItemsState(false);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.ItemsSell:
                    foreach (GameObject go in this.shopSellItems)
                        go.SetActive(false);
                    this.PlayerItemsState(true);
                    this.itemShop.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Equipment:
                    this.PlayerEquipmentState(false);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.EquipmentSell:
                    foreach (GameObject go in this.shopSellItems)
                        go.SetActive(false);
                    this.equipmentShop.SetActive(true);
                    this.PlayerEquipmentState(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Spells:
                    this.playerSpells.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.ItemShop:
                    foreach (GameObject go in this.shopSellItems)
                        go.SetActive(true);
                    this.itemShop.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.SpellShop:
                    this.spellShop.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.EquipmentShop:
                    foreach (GameObject go in this.shopSellItems)
                        go.SetActive(true);
                    this.equipmentShop.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Inn:
                    this.inn.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Bank:
                    this.bank.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Map:
                    this.MapOpened = true;
                    this.playerMap.SetActive(true);
                    this.ActivateMenuButtons(true);
                    this.navControls.SetActive(true);
                    this.playerStats.SetActive(true);
                    break;
                case UIState.Title:
                    this.titleScreen.SetActive(true);
                    break;
                case UIState.Settings:
                    this.settingsScreen.SetActive(true);
                    break;
                case UIState.Credits:
                    this.creditsScreen.SetActive(true);
                    this.creditsScreen.GetComponent<CreditsScreen>().ShowBackButton(true);
                    break;
                case UIState.CreditsNoBack:
                    this.creditsScreen.SetActive(true);
                    this.creditsScreen.GetComponent<CreditsScreen>().ShowBackButton(false);
                    break;
                case UIState.DungeonFloorChoice:
                    this.dungeonChoiceScreen.SetActive(true);
                    break;
				case UIState.Glossary:
					this.GlossaryScreen.SetActive(true);
					this.GlossaryScreen.GetComponent<GlossaryScreen>().ShowBackButton(true);
					break;

                case UIState.SaveMenu:
                    SaveMenu saveMenu = this.saveMenuScreen.GetComponent<SaveMenu>();
                    if (saveMenu != null)
                        saveMenu.Mode = SaveMenu.ActionMode.Save;
                    this.saveMenuScreen.SetActive(true);
                    break;

                case UIState.LoadMenu:
                    SaveMenu loadMenu = this.saveMenuScreen.GetComponent<SaveMenu>();
                    if (loadMenu != null)
                        loadMenu.Mode = SaveMenu.ActionMode.Load;
                    this.saveMenuScreen.SetActive(true);
                    break;
                case UIState.OpeningScene:
                    this.openingScene.SetActive(true);
                    break;
                case UIState.EndingScene:
                    this.endingScene.SetActive(true);
                    break;
            }
        }

        private void ActivateMenuButtons(bool p)
        {
            this.dungeonMenu.SetActive(p);
        }

        private void PlayerEquipmentState(bool sell)
        {
            PlayerEquipmentMenu equipMenu = this.playerEquipment.GetComponent<PlayerEquipmentMenu>();
            if (equipMenu != null)
                equipMenu.Sell = sell;
            this.playerEquipment.SetActive(true);
            this.playerStats.SetActive(true);
        }

        private void PlayerItemsState(bool sell)
        {
            PlayerItemMenu itemMenu = this.playerItems.GetComponent<PlayerItemMenu>();
            if (itemMenu != null)
                itemMenu.Sell = sell;
            this.playerItems.SetActive(true);
            this.playerStats.SetActive(true);
        }

        public void ActivateDungeonMenuState()
        {
            this.UpdateUIState(UIState.Menu);
        }

        public void ActivatePreviousState()
        {
            this.UpdateUIState(this.previousState);
        }

        public void DeactivateAll()
        {
            this.playerStats.SetActive(false);
            this.navControls.SetActive(false);
            this.battleCommands.SetActive(false);
            this.battleIndicatorPanel.SetActive(false);
            this.gameOver.SetActive(false);
            this.dungeonMenu.SetActive(false);
            this.ActivateMenuButtons(false);
            this.playerSpells.SetActive(false);
            this.playerItems.SetActive(false);
            this.playerStatus.SetActive(false);
            this.playerEquipment.SetActive(false);
            this.playerMap.SetActive(false);
            this.townOutsidePanel.SetActive(false);
            this.enemyName.SetActive(false);

            this.equipmentShop.SetActive(false);
            this.spellShop.SetActive(false);
            this.itemShop.SetActive(false);
            this.inn.SetActive(false);
            this.bank.SetActive(false);
            this.mapButton.SetActive(false);
            this.titleScreen.SetActive(false);
            this.creditsScreen.SetActive(false);
			this.GlossaryScreen.SetActive(false);
            this.settingsScreen.SetActive(false);
            this.dungeonChoiceScreen.SetActive(false);
            this.saveMenuScreen.SetActive(false);
            this.interactionPanel.SetActive(false);
            this.openingScene.SetActive(false);
            this.endingScene.SetActive(false);
        }

        public void BattleCommands(bool active)
        {
            this.battleCommands.SetActive(active);
        }

        public void PlayerSpells(bool active)
        {
            this.playerSpells.SetActive(active);
        }

        public void PlayerItems(bool active)
        {
            this.playerItems.SetActive(active);
        }

        public void PlayerEquipment(bool active)
        {
            this.playerEquipment.SetActive(active);
        }

        public void GameOver(bool active)
        {
            this.gameOver.SetActive(active);
        }

        public void PlayerStatus(bool active)
        {
            this.playerStatus.SetActive(active);
        }

        public void Back()
        {
            this.LoadCurrentState();
        }
    }
}
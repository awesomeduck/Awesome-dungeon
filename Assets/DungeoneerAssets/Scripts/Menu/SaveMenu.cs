using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class SaveMenu : PaginationBase<SaveGameState>
    {
        public SaveGameComponent saveComponent;
        public ActionMode Mode { get; set; }
        public Text txtTitle;
        public Text txtMessage;
        public GameObject btnNew;

        protected override void LoadContent()
        {
            SaveGameManager saveManager = new SaveGameManager();
            this.content = saveManager.GetAllSaves();
            if ((this.content.Count % 2) != 0)
                this.content.Add(null);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            switch (this.Mode)
            {
                case ActionMode.Save:
                    txtTitle.text = "Save Game";
                    btnNew.SetActive(true);
                    break;
                case ActionMode.Load:
                    txtTitle.text = "Load Game";
                    btnNew.SetActive(false);
                    break;
            }
        }
        protected override void TemplateInitialize(UnityEngine.GameObject templateInstance, SaveGameState item)
        {
            SaveGamePanel panel = templateInstance.GetComponent<SaveGamePanel>();
            if (panel != null)
            {
                if (item != null)
                {
                    panel.SaveState = item;
                    panel.Clicked += panel_Clicked;
                }
                else
                    panel.ClearDisplay();
            }
        }

        void panel_Clicked(SaveGameState obj)
        {
            switch (this.Mode)
            {
                case ActionMode.Save:
                    this.saveComponent.Save(obj.Index);
                    this.txtMessage.text = "Game Saved!";
                    this.Refresh();
                    break;
                case ActionMode.Load:
                    this.saveComponent.Load(obj.Index);
                    break;
            }
        }

        public void SaveNew()
        {
            this.saveComponent.SaveNew();
            this.Refresh();
            this.GoToLastPage();

        }

        protected override void DisposeItem(UnityEngine.Transform item)
        {
            SaveGamePanel panel = item.GetComponent<SaveGamePanel>();
            if (panel != null)
            {
                if (panel.SaveState != null)
                {
                    panel.SaveState = null;
                    panel.Clicked -= panel_Clicked;
                }
            }
        }

        protected override string GetName(SaveGameState item)
        {
            if (item != null)
                return item.Index.ToString();
            else return "empty";
        }


        public enum ActionMode
        {
            Save,
            Load
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public abstract class PlayerMenuBase : MonoBehaviour
    {
        private const int ItemsPerPage = 8;
        public GameObject itemTemplatePrefab;
        public Transform stockPanel;
        public Text messageText;
        public Player player;
        protected int page;
        protected List<DefinitionBase> fullStock;
        public GameObject btnNextPage;
        public GameObject btnPrevPage;

        protected virtual string ActionText { get { return null; } }

        protected void AddItemsToPanel(List<DefinitionBase> items)
        {
            foreach (DefinitionBase def in items)
            {
                GameObject itemSlot = GameObject.Instantiate(this.itemTemplatePrefab) as GameObject;
                if (itemSlot != null)
                {
                    itemSlot.name = def.Name + "panel";
                    NameCostPanel panel = itemSlot.GetComponent<NameCostPanel>();
                    panel.Content = def;
                    panel.txtCost.text = this.GetCost(def);
                    panel.txtName.text = def.DisplayName;
                    panel.OnDescriptionRequested += DescriptionRequestedHandler;
                    panel.OnConfirmRequested += ConfirmationRequestedHandler;
                    if (this.ActionText != null)
                        panel.btnConfirm.UpdateText(this.ActionText);
                    itemSlot.transform.SetParent(this.stockPanel);
                    itemSlot.transform.localEulerAngles = Vector3.zero;
                    ((RectTransform)itemSlot.transform).anchoredPosition3D = new Vector3(((RectTransform)itemSlot.transform).anchoredPosition3D.x,
                        ((RectTransform)itemSlot.transform).anchoredPosition3D.y, 0);
                    itemSlot.transform.localScale = new Vector3(1, 1, 1);
                }

            }
        }

        protected virtual string GetCost(DefinitionBase def)
        {
            return def.Cost.ToString();
        }

        protected abstract void LoadFullStock();

        private List<DefinitionBase> GetCurrentPageStock()
        {
            return this.fullStock
                .Skip(ItemsPerPage * (this.page - 1))
                .Take(ItemsPerPage)
                .ToList();
        }

        protected virtual void OnEnable()
        {
            this.page = 1;
            this.LoadFullStock();
            this.LoadPage();

        }

        private void LoadPage()
        {
            this.Clear();
            List<DefinitionBase> defs = this.GetCurrentPageStock();
            if (defs != null && defs.Count > 0)
                this.AddItemsToPanel(defs);
            else
                messageText.text = "Sorry, we don't have anything in stock right now. Please come back later.";

        }

        private void Clear()
        {
            foreach (Transform child in this.stockPanel)
            {
                NameCostPanel panel = child.GetComponent<NameCostPanel>();
                if (panel != null)
                {
                    panel.OnDescriptionRequested -= DescriptionRequestedHandler;
                    panel.OnConfirmRequested -= ConfirmationRequestedHandler;
                    panel.Content = null;
                }
                GameObject.Destroy(child.gameObject);
            }
        }


        protected virtual void OnDisable()
        {
            this.Clear();
        }

        protected virtual void DescriptionRequestedHandler(object def)
        {
            this.messageText.text = ((DefinitionBase)def).Description;
        }

        protected virtual void ConfirmationRequestedHandler(object def)
        { }


        protected virtual bool NextPageAvailable()
        {
            return this.fullStock.Count > ItemsPerPage * this.page;
        }

        protected virtual bool PrevPageAvailable()
        {
            return this.page > 1;
        }

        public void OnNextPage()
        {
            if (this.NextPageAvailable())
            {
                this.page++;
                this.LoadPage();
                this.btnNextPage.SetActive(this.NextPageAvailable());
                this.btnPrevPage.SetActive(this.PrevPageAvailable());
            }
        }

        public void OnPreviousPage()
        {
            if (this.PrevPageAvailable())
            {
                if (this.page > 1)
                {
                    this.page--;
                    this.LoadPage();
                    this.btnNextPage.SetActive(this.NextPageAvailable());
                    this.btnPrevPage.SetActive(this.PrevPageAvailable());
                }
            }
        }
    }
}
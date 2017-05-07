using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public abstract class ShopBase<T> : PaginationBase<T> where T : class
    {
        public Text messageText;
        public Player player;

        protected abstract string GetCost(T item);
        protected abstract string GetDisplayName(T item);
        protected virtual Color GetTextColor(T item) { return Color.white; }
        protected virtual string GetActionText(T def) { return "Buy"; }

        protected override void TemplateInitialize(GameObject templateInstance, T item)
        {
            NameCostPanel panel = templateInstance.GetComponent<NameCostPanel>();
            panel.Content = item;
            panel.txtCost.text = this.GetCost(item);
            panel.txtCost.color = this.GetTextColor(item);
            panel.txtName.text = this.GetDisplayName(item);
            panel.txtName.color = this.GetTextColor(item);
            string action = this.GetActionText(item);
            if (!string.IsNullOrEmpty(action))
            {
                panel.btnConfirm.gameObject.SetActive(true);
                panel.btnConfirm.UpdateText(action);
            }
            else
                panel.btnConfirm.gameObject.SetActive(false);
            panel.OnDescriptionRequested += DescriptionRequestedHandler;
            panel.OnConfirmRequested += ConfirmationRequestedHandler;
        }

        protected void RemoveItem(T def)
        {
            foreach (Transform child in this.stockPanel.transform)
            {
                if (child != null)
                {
                    NameCostPanel panel = child.GetComponent<NameCostPanel>();
                    if (panel != null)
                    {
                        if (panel.Content == def)
                            GameObject.Destroy(child.gameObject);
                    }
                }
            }
        }

        protected override void DisposeItem(Transform item)
        {
            NameCostPanel panel = item.GetComponent<NameCostPanel>();
            if (panel != null)
            {
                panel.OnDescriptionRequested -= DescriptionRequestedHandler;
                panel.OnConfirmRequested -= ConfirmationRequestedHandler;
                panel.Content = null;
            }
        }

        protected virtual void DescriptionRequestedHandler(object def)
        {
            this.messageText.text = ((DefinitionBase)def).Description;
        }

        protected virtual void ConfirmationRequestedHandler(object def)
        { }
    }
}
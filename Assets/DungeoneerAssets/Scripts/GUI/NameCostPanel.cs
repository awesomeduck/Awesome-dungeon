using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace DungeoneerAssets
{
    public class NameCostPanel : MonoBehaviour
    {
        public event Action<object> OnConfirmRequested;
        public event Action<object> OnDescriptionRequested;

        public Text txtName;
        public Text txtCost;
        public Button btnConfirm;

        public object Content { get; set; }

        public void OnConfirm()
        {
            Action<object> temp = this.OnConfirmRequested;
            if (temp != null)
                temp(this.Content);
        }

        public void OnShowDescription()
        {
            Action<object> temp = this.OnDescriptionRequested;
            if (temp != null)
                temp(this.Content);
        }
    }
}
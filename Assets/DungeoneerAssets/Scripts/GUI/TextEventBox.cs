using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class TextEventBox : MonoBehaviour
    {     
        [SerializeField]
        private Text txtDisplay;

        private string[] textLines;
        private int index; 

        void Start()
        {
            if (this.txtDisplay == null) Debug.LogError("txtDisplay must be attached.");
        }

        public void OnNextPressed()
        {
            this.index++;
            if (this.index < this.textLines.Length)
                this.txtDisplay.text = this.textLines[index];
            else
                this.gameObject.SetActive(false);
        }

        public void ShowMessage(string[] text)
        {
            if(text == null || text.Length == 0)
                Debug.Log("TextEventBox.ShowMessage called without any text provided.");

            this.textLines = text;
            this.index = 0;
            this.txtDisplay.text = text[0];
            this.gameObject.SetActive(true);
        }
    }
}
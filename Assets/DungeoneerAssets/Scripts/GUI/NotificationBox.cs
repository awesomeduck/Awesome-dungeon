using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class NotificationBox : MonoBehaviour
    {
        public GameObject panel;
        public Text textBox;

        public void Show()
        {
            this.panel.SetActive(true);
        }

        public void Hide()
        {
            this.panel.SetActive(false);
        }

        public void AddText(string msg)
        {
            if (!string.IsNullOrEmpty(textBox.text))
                textBox.text += Environment.NewLine;
            textBox.text += msg;
        }

        public void ClearText()
        {
            textBox.text = "";
        }

        public IEnumerator DelayHide(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            this.Hide();
        }

        public IEnumerator PopSingleMessageCoroutine(string message, float seconds)
        {
            this.ClearText();
            this.AddText(message);
            this.Show();
            yield return new WaitForSeconds(seconds);
            this.Hide();
            this.ClearText();
        }
    }
}
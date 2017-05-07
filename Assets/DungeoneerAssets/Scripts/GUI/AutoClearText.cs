using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class AutoClearText : MonoBehaviour
    {
        private void OnEnable()
        {
            Text textBox = this.GetComponent<Text>();
            textBox.text = "";
        }
    }
}
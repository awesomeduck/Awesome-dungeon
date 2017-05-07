using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class ScanResultsDisplay : MonoBehaviour
    {
        public GameObject panel;
        public Text txtResults;

        public void Show(bool val)
        {
            panel.SetActive(val);
        }

        public void SetText(string val)
        {
            txtResults.text = val;
        }
    }
}
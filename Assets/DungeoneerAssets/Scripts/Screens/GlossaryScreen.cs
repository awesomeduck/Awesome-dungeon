using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class GlossaryScreen : MonoBehaviour
    {
        public GameObject btnBack;

        public void ShowBackButton(bool b)
        {
            btnBack.SetActive(b);
        }
    }
}
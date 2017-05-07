using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class DungeonFloorChoice : PaginationBase<string>
    {
        public Player player;
        public FloorManager floorManager;

        private int SortFloorNames(string a, string b)
        {
            string aNum = a.Substring(3, a.Length - 3);
            string bNum = b.Substring(3, b.Length - 3);
            int aFloor = int.Parse(aNum);
            int bFloor = int.Parse(bNum);

            if (aFloor < bFloor)
                return -1;
            else if (bFloor > aFloor)
                return 1;
            else return 0;
        }

        protected override void LoadContent()
        {
            this.content = this.player.ExplorationInfo.ExploredAreas.Keys
                .ToList();
            this.content.Sort(SortFloorNames);
        }

        protected override void TemplateInitialize(GameObject templateInstance, string item)
        {
            Button button = templateInstance.GetComponent<Button>();
            if (button != null)
            {
                button.UpdateText(this.GetDisplayName(item));
                button.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
                    {
                        this.floorManager.GoToFloor(item);
                    }));
            }
        }

        protected override void DisposeItem(UnityEngine.Transform item)
        {
            Button button = item.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        protected override string GetName(string item)
        {
            return item;
        }

        protected string GetDisplayName(string item)
        {
            TextAsset txt = Resources.Load<TextAsset>(item);
            if (txt != null)
            {
                XDocument xDoc = XDocument.Parse(txt.text);
                if (xDoc != null)
                    return xDoc.Element("Data").Element("DisplayName").Value;
            }
            return item;
        }
    }
}
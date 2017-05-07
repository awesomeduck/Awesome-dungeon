using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace DungeoneerAssets
{
    [Serializable]
    public class SaveGameState
    {
        public DateTime SaveDate { get; set; }
        public DateTime GameStartedDate { get; set; }

        [XmlIgnore]
        public TimeSpan PlayTime { get; set; }

        [Browsable(false)]
        [XmlElement(DataType = "duration", ElementName = "PlayTime")]
        public string PlayTimeString
        {
            get { return XmlConvert.ToString(this.PlayTime); }
            set { this.PlayTime = string.IsNullOrEmpty(value) ? TimeSpan.Zero : XmlConvert.ToTimeSpan(value); }
        }

        public int Index { get; set; }
        public PlayerStats PlayerStats { get; set; }
        public Inventory PlayerInventory { get; set; }
        public List<string> PlayerSpells { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public string CurrentFloorName { get; set; }
        public List<Interaction> FinishedInteractions { get; set; }
        public List<BossCompletion> BossCompletions { get; set; }
        public List<FloorExplorationData> ExplorationData { get; set; }

    }
}
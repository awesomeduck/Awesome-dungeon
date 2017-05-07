using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    [Serializable]
    public class ExplorationData
    {
        public Dictionary<string, List<MapSpaceInfo>> ExploredAreas { get; set; }

        public ExplorationData()
        {
            this.ExploredAreas = new Dictionary<string, List<MapSpaceInfo>>();
        }

        private List<MapSpaceInfo> GetFloorData(string floorName)
        {
            List<MapSpaceInfo> floorData = null;
            if (!this.ExploredAreas.ContainsKey(floorName))
            {
                floorData = new List<MapSpaceInfo>();
                this.ExploredAreas.Add(floorName, floorData);
            }
            else
                floorData = this.ExploredAreas[floorName];
            return floorData;
        }

        /// <summary>
        /// Adds the currently positioned space to the mapped data.
        /// </summary>
        public void AddExploredSpace(string floorName, Vector2 pos)
        {
            pos = pos.Round(0);
            List<MapSpaceInfo> floorData = this.GetFloorData(floorName);

            if (!floorData.Any(x => x.Position.x == pos.x && x.Position.y == pos.y))
            {
                floorData.Add(new MapSpaceInfo { Position = pos });
            }
        }

        public void AddStairsUp(string floorName, Vector2 pos)
        {
            pos = pos.Round(0);
            List<MapSpaceInfo> floorData = this.GetFloorData(floorName);
            MapSpaceInfo info = floorData.FirstOrDefault(x => x.Position.x == pos.x && x.Position.y == pos.y);
            if (info != null)
            {
                if (info is StairsUpMapSpaceInfo)
                    return;

                floorData.Remove(info);
            }

            floorData.Add(new StairsUpMapSpaceInfo { Position = pos });
        }

        public void AddStairsDown(string floorName, Vector2 pos)
        {
            pos = pos.Round(0);
            List<MapSpaceInfo> floorData = this.GetFloorData(floorName);
            MapSpaceInfo info = floorData.FirstOrDefault(x => x.Position.x == pos.x && x.Position.y == pos.y);

            if (info != null)
            {
                if (info is StairsDownMapSpaceInfo)
                    return;
                floorData.Remove(info);
            }
            floorData.Add(new StairsDownMapSpaceInfo { Position = pos });
        }
    }
}
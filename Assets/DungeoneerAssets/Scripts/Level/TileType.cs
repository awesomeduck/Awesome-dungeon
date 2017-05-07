using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;

namespace DungeoneerAssets
{
    public enum TileType : byte
    {
        Wall,
        Floor,
        StairsUp,
        StairsDown,
        Treasure,
        Boss, 
        TextEvent
    }

    public class TileInfo
    {
        public TileType Type { get; set; }
        public Color32 ColorData { get; set; }
    }

    [XmlInclude(typeof(StairsUpMapSpaceInfo))]
    [XmlInclude(typeof(StairsDownMapSpaceInfo))]
    public class MapSpaceInfo
    {
        public Vector2 Position { get; set; }
    }

    public class StairsUpMapSpaceInfo : MapSpaceInfo
    { }

    public class StairsDownMapSpaceInfo : MapSpaceInfo
    { }
}
using UnityEngine;
using System.Collections;

namespace DungeoneerAssets
{
    public class Floor
    {
        private TileInfo[,] tiles;

        public Vector2 Size { get; private set; }

        public Floor(int height, int width)
        {
            this.Size = new Vector2(width, height);
            tiles = new TileInfo[width, height];
        }

        public void SetTile(int x, int y, TileInfo t)
        {
            tiles[x, y] = t;
        }

        public TileInfo GetTile(int x, int y)
        {
            return tiles[x, y];
        }

        public static TileType ColorToType(Color32 col)
        {
            if (col == Color.black) // wall
                return TileType.Wall;
            else if (col == Color.white) // floor
                return TileType.Floor;
            else if (col == Color.green) // stairs up (starting point)
                return TileType.StairsUp;
            else if (col == Color.red) // stairs down (end point)
                return TileType.StairsDown;
            else if (col == Color.magenta)
                return TileType.Boss;
            // use blue scale to determine what kind of treasure it is. 
            else if (col.b > 0)
                return TileType.Treasure;
            else if (col.r > 0)
                return TileType.TextEvent;
            return 0;
        }
    }
}
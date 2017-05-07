using UnityEngine;
using System.Collections;
using System;

namespace DungeoneerAssets
{
    public static class FloorLoader
    {
        /// <summary>
        /// Takes an image file and loads the floor info based on the pixels, similar to a heightmap. 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Floor LoadFloor(string fileName)
        {
            Floor floor = null;
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("FileName");

            Texture2D bmp = Resources.Load<Texture2D>(fileName);
            if (bmp == null)
                throw new ArgumentException("FileName " + fileName + " not found in the Resources directory");

            floor = new Floor(bmp.height, bmp.width);
            for (int i = 0; i < bmp.width; i++)
                for (int j = 0; j < bmp.height; j++)
                {
                    Color32 c = bmp.GetPixel(i, j);
                    TileType t = Floor.ColorToType(c);
                    TileInfo info = new TileInfo { Type = t, ColorData = c };
                    floor.SetTile(i, j, info);
                }

            return floor;
        }
    }
}
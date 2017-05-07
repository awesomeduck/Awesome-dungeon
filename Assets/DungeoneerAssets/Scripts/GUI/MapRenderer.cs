using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class MapRenderer
    {
        public Texture2D Texture { get; set; }
        public int LocalScale { get; private set; }
        private readonly int mapSize = 30;

        public void Initialize()
        {
            this.Texture = new Texture2D(this.mapSize, this.mapSize, TextureFormat.ARGB32, false);
        }

        public void DrawFloor(ExplorationData data, string floorName, int scaledHeight)
        {
            if (!data.ExploredAreas.ContainsKey(floorName))
                throw new ArgumentException("No Map Data");
            this.LocalScale = (int)(scaledHeight / this.mapSize);
            if (this.LocalScale < 1)
                throw new InvalidOperationException("Target scale is too small.");

            if (this.Texture.height != scaledHeight)
                this.Texture.Resize(scaledHeight, scaledHeight); // always square floormaps

            this.Clear();
            List<MapSpaceInfo> mapInfo = data.ExploredAreas[floorName];
            foreach (MapSpaceInfo info in mapInfo)
            {
                if (this.LocalScale > 1)
                {
                    int newX = (int)Math.Round(info.Position.x) * this.LocalScale;
                    int newY = (int)Math.Round(info.Position.y) * this.LocalScale;
                    for (int x = newX; x < newX + this.LocalScale; x++)
                        for (int y = newY; y < newY + this.LocalScale; y++)
                            this.Texture.SetPixel(x, y, Color.white);
                }
                else if (this.LocalScale == 1)
                    this.Texture.SetPixel((int)info.Position.x, (int)info.Position.y, Color.white);
            }
            this.Texture.Apply();
        }

        internal void DrawOverlay(Transform transform, Texture2D tex)
        {
            if (this.LocalScale == 0)
                throw new InvalidOperationException("Must call DrawMap first!");

            this.DrawOverlay(new Vector2(transform.position.x, transform.position.z), tex);

        }

        public void DrawOverlay(Vector2 position, Texture2D texture)
        {
            int i = 0;
            int j = 0;
            int newX = (int)Math.Round(position.x) * this.LocalScale;
            int newY = (int)Math.Round(position.y) * this.LocalScale;
            for (int x = newX; x < newX + texture.width; x++)
            {
                for (int y = newY; y < newY + texture.height; y++)
                {
                    Color32 c = texture.GetPixel(i, j);
                    if (c.a != 0)
                    {
                        this.Texture.SetPixel(
                            x - (texture.width / 2) + this.LocalScale / 2,
                            y - (texture.height / 2) + this.LocalScale / 2,
                            c);
                    }
                    j++;
                }
                i++;
                j = 0;

            }
            this.Texture.Apply();
        }

        public void Clear()
        {
            for (int i = 0; i < this.Texture.width; i++)
                for (int j = 0; j < this.Texture.height; j++)
                    this.Texture.SetPixel(i, j, Color.black);
            this.Texture.Apply();
        }
    }
}
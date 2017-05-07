using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class MonsterImageRepository : MonoBehaviour
    {
        private Dictionary<string, Sprite> sprites;

        public void Start()
        {
            this.sprites = new Dictionary<string, Sprite>();
        }


        public Sprite GetSprite(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                throw new ArgumentException("imageName");

            if (this.sprites.ContainsKey(imageName))
                return this.sprites[imageName];
            else
            {
                Sprite sp = Resources.Load<Sprite>("EnemySprites/" + imageName);
                if (sp != null)
                {
                    this.sprites.Add(imageName, sp);
                    return sp;
                }
                else
                    Debug.LogError("Sprite could not be loaded from resources: " + imageName);
            }
            return null;
        }
    }
}
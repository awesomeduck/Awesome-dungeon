using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class PlayerMap : MonoBehaviour
    {
        public Player player;
        public PlayerNavigation playerNav;
        public FloorManager floorManager;
        public Image mapImage;
        //public Image playerImage;
        public Texture2D playerSprite;
        public Texture2D stairsUpSprite;
        public Texture2D stairsDownSprite;
        public Image compassImage;
        public Sprite compassNorth;
        public Sprite compassWest;
        public Sprite compassEast;
        public Sprite compassSouth;
        public MapRenderer mapRenderer;
        public Text txtDisplayName;

        private void OnEnable()
        {
            if (this.mapRenderer == null)
            {
                this.mapRenderer = new MapRenderer();
                this.mapRenderer.Initialize();
            }

            this.DrawMap();
            this.UpdateCompass();
            this.playerNav.PlayerMoved += playerNav_PlayerMoved;
            this.playerNav.PlayerTurned += playerNav_PlayerTurned;
            this.floorManager.FloorChanged += floorManager_FloorChanged;
        }

        void playerNav_PlayerTurned(Vector3 obj)
        {
            if (this.gameObject.activeInHierarchy)
                this.UpdateCompass();
        }

        void floorManager_FloorChanged()
        {
            if (this.gameObject.activeInHierarchy)
                this.DrawMap();
        }

        void playerNav_PlayerMoved(Vector3 obj)
        {
            if (this.gameObject.activeInHierarchy)
                this.DrawMap();
        }

        private void OnDisable()
        {
            this.playerNav.PlayerMoved -= playerNav_PlayerMoved;
            this.playerNav.PlayerTurned -= playerNav_PlayerTurned;
            this.floorManager.FloorChanged -= floorManager_FloorChanged;
        }


        private void DrawMap()
        {
            // do this first to get the layout scaling factor. 
            this.txtDisplayName.text = this.floorManager.displayName;
            this.mapImage.preserveAspect = true;
            this.mapImage.sprite = Sprite.Create(this.mapRenderer.Texture, new Rect(0, 0, this.mapRenderer.Texture.width, this.mapRenderer.Texture.height), Vector2.zero);
            float scaledHeight = this.mapImage.rectTransform.GetHeight();

            this.StartCoroutine(this.DrawMap(scaledHeight));
        }

        private IEnumerator DrawMap(float scaledHeight)
        {
            yield return new WaitForEndOfFrame();
            scaledHeight = this.mapImage.rectTransform.GetHeight();
            this.mapRenderer.DrawFloor(this.player.ExplorationInfo, this.floorManager.floorName, (int)scaledHeight);
            this.mapRenderer.DrawOverlay(this.player.transform, this.playerSprite);

            StairsUpMapSpaceInfo stairsUp = this.player.ExplorationInfo.ExploredAreas[this.floorManager.floorName].OfType<StairsUpMapSpaceInfo>().FirstOrDefault();
            StairsDownMapSpaceInfo stairsDown = this.player.ExplorationInfo.ExploredAreas[this.floorManager.floorName].OfType<StairsDownMapSpaceInfo>().FirstOrDefault();

            if (stairsUp != null)
                this.mapRenderer.DrawOverlay(stairsUp.Position, this.stairsUpSprite);
            if (stairsDown != null)
                this.mapRenderer.DrawOverlay(stairsDown.Position, this.stairsDownSprite);

            this.mapImage.sprite = Sprite.Create(this.mapRenderer.Texture, new Rect(0, 0, this.mapRenderer.Texture.width, this.mapRenderer.Texture.height), Vector2.zero);
        }

        private void UpdateCompass()
        {
            if (this.compassNorth != null && this.compassWest != null)
            {
                if (this.player.transform.forward.x < 0)
                    this.compassImage.sprite = this.compassWest;
                else if (this.player.transform.forward.x > 0)
                    this.compassImage.sprite = this.compassEast;
                else if (this.player.transform.forward.z > 0)
                    this.compassImage.sprite = this.compassNorth;
                else if (this.player.transform.forward.z < 0)
                    this.compassImage.sprite = this.compassSouth;
            }
        }
    }
}
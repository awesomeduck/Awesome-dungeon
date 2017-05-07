using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace DungeoneerAssets
{
    public class PlayerEffectIndicators : MonoBehaviour
    {
        public Image[] imageControls;

        public Sprite focusSprite;
        public Sprite extendedGuardSprite;
        public Sprite perfectGuardSprite;

        private bool isFocused;
        private bool isExtendedGuard;
        private bool isPerfectGuard;

        void Start()
        {
            if (this.imageControls == null || this.imageControls.Length == 0)
                Debug.LogError("ImageControls");
            if (this.focusSprite == null)
                Debug.LogError("Focus Sprite");
            if (this.extendedGuardSprite == null)
                Debug.LogError("ExtendedGuardSprite");
            if (this.perfectGuardSprite == null)
                Debug.LogError("PerfectGuardSprite");
        }

        public void AddFocus()
        {
            if (!this.isFocused)
            {
                if (this.CanAddSprite(this.focusSprite))
                    this.AddSprite(this.focusSprite);
                this.isFocused = true;
            }
        }

        public void AddExtendedGuard()
        {
            if (!this.isExtendedGuard)
            {
                if (this.CanAddSprite(this.extendedGuardSprite))
                    this.AddSprite(this.extendedGuardSprite);
                this.isExtendedGuard = true;
            }
        }

        public void AddPerfectGuard()
        {
            if (!this.isPerfectGuard)
            {
                if (this.CanAddSprite(this.perfectGuardSprite))
                    this.AddSprite(this.perfectGuardSprite);
                this.isPerfectGuard = true;
            }
        }

        public void RemoveFocus()
        {
            if (this.isFocused)
            {
                this.RemoveSprite(this.focusSprite);
                this.isFocused = false;
            }
        }

        public void RemoveExtendedGuard()
        {
            if (this.isExtendedGuard)
            {
                this.RemoveSprite(this.extendedGuardSprite);
                this.isExtendedGuard = false;
            }
        }

        public void RemovePerfectGuard()
        {
            if (this.isPerfectGuard)
            {
                this.RemoveSprite(this.perfectGuardSprite);
                this.isPerfectGuard = false;
            }
        }

        private void AddSprite(Sprite sp)
        {
            foreach (Image img in this.imageControls)
            {
                if (img.sprite == null)
                {
                    img.sprite = sp;
                    break;
                }
            }
        }

        private void RemoveSprite(Sprite sp)
        {
            foreach (Image img in this.imageControls)
            {
                if (img.sprite == sp)
                    img.sprite = null;
            }
            this.FillInSpaces();
        }

        private bool CanAddSprite(Sprite sp)
        {
            foreach (Image img in this.imageControls)
            {
                if (img.sprite == sp)
                {
                    return false;
                }
            }
            return true;
        }

        public void ClearIndicators()
        {
            this.isFocused = false;
            this.isExtendedGuard = false;
            this.isPerfectGuard = false;
            foreach (Image img in this.imageControls)
                img.sprite = null;
        }

        public void FillInSpaces()
        {
            for (int i = 0; i < this.imageControls.Length - 1; i++)
            {
                if (this.imageControls[i].sprite == null && this.imageControls[i + 1].sprite != null)
                {
                    this.imageControls[i].sprite = this.imageControls[i + 1].sprite;
                    this.imageControls[i + 1].sprite = null;
                }

            }
        }
    }
}
using UnityEngine;
using System.Collections;
using System;

namespace DungeoneerAssets
{
    public struct NavigationState
    {
        public bool LeftOpen { get; set; }
        public bool RightOpen { get; set; }
        public bool ForwardOpen { get; set; }
        public bool BackOpen { get; set; }

        public override bool Equals(object obj)
        {
            NavigationState compareTo = (NavigationState)obj;
            return this.LeftOpen == compareTo.LeftOpen &&
                this.RightOpen == compareTo.RightOpen &&
                this.ForwardOpen == compareTo.ForwardOpen &&
                this.BackOpen == compareTo.BackOpen;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + LeftOpen.GetHashCode();
            hash = (hash * 7) + RightOpen.GetHashCode();
            hash = (hash * 7) + ForwardOpen.GetHashCode();
            hash = (hash * 7) + BackOpen.GetHashCode();
            return hash;
        }

        public void Clear()
        {
            this.RightOpen = this.LeftOpen = this.ForwardOpen = this.BackOpen = false;
        }
    }

    public class PlayerNavigation : MonoBehaviour
    {
        public event Action<Vector3> PlayerMoved;
        public event Action<Vector3> PlayerTurned;
        public FloorManager floorManager;
        DateTime lastInput;
        public float timeBetweenMoves;
        public bool CanResetBattleFlag = true;
        public InteractionPanel interactionPanel;
        public AudioManager audioManager;
        public Player player;
        public bool pauseAtIntersectionsEnabled;
        public SettingsManager settings;
        private bool forwardFlag;
        private bool backFlag;
        private bool leftFlag;
        private bool rightFlag;
        private bool waitForKeyUp;
        private bool waitForButtonUp;
        private bool canWaitAtIntersection;
        private bool waitingAtIntersection;

        private NavigationState prevNavState;
        private NavigationState curNavState;

        public void TurnRight()
        {
            if (!this.enabled)
                return;
            this.transform.Rotate(Vector3.up, 90);
            this.transform.forward = this.transform.forward.Round(0);
            this.CheckFacingDirection();
            this.OnTurned();
        }

        public void TurnLeft()
        {
            if (!this.enabled)
                return;
            this.transform.Rotate(Vector3.up, -90);
            this.transform.forward = this.transform.forward.Round(0);
            this.CheckFacingDirection();
            this.OnTurned();
        }

        public void Forward()
        {
            if (!this.enabled)
                return;

            if ((DateTime.Now - this.lastInput).TotalMilliseconds > timeBetweenMoves)
            {
                if (this.CheckDirection(this.transform.forward))
                {
                    this.transform.position += this.transform.forward;
                    this.lastInput = DateTime.Now;
                    this.OnMoved();
                    this.CheckFacingDirection();
                    this.audioManager.PlaySFX(SFX.Footstep);
                }
                else
                    this.audioManager.PlaySFX(SFX.WallBump);
            }
        }

        public void Left()
        {
            if (!this.enabled)
                return;
            if ((DateTime.Now - this.lastInput).TotalMilliseconds > timeBetweenMoves)
            {
                if (this.CheckDirection(-this.transform.right))
                {
                    this.transform.position -= this.transform.right;
                    this.lastInput = DateTime.Now;
                    this.OnMoved();
                    this.CheckFacingDirection();
                    this.audioManager.PlaySFX(SFX.Footstep);
                }
                else
                    this.audioManager.PlaySFX(SFX.WallBump);
            }
        }

        public void Right()
        {
            if (!this.enabled)
                return;
            if ((DateTime.Now - this.lastInput).TotalMilliseconds > timeBetweenMoves)
            {
                if (this.CheckDirection(this.transform.right))
                {
                    this.transform.position += this.transform.right;
                    this.lastInput = DateTime.Now;
                    this.OnMoved();
                    this.CheckFacingDirection();
                    this.audioManager.PlaySFX(SFX.Footstep);
                }
                else
                    this.audioManager.PlaySFX(SFX.WallBump);
            }
        }

        public void Back()
        {
            if (!this.enabled)
                return;
            if ((DateTime.Now - this.lastInput).TotalMilliseconds > timeBetweenMoves)
            {
                if (this.CheckDirection(-this.transform.forward))
                {
                    this.transform.position -= this.transform.forward;
                    this.lastInput = DateTime.Now;
                    this.OnMoved();
                    this.CheckFacingDirection();
                    this.audioManager.PlaySFX(SFX.Footstep);
                }
                else
                    this.audioManager.PlaySFX(SFX.WallBump);
            }
        }

        private void NormalizeTransform()
        {
            this.transform.position = this.transform.position.RoundLaterals(0);
            this.transform.forward = this.transform.forward.Round(0);
        }

        public void OnMoved()
        {
            this.NormalizeTransform();
            if (this.CanResetBattleFlag)
                BattleManager.AllowBattle = true;
            this.floorManager.UpdateInteractableDirection();
            this.player.ExplorationInfo.AddExploredSpace(this.floorManager.floorName, new Vector2(this.player.transform.position.x, this.player.transform.position.z));
            this.canWaitAtIntersection = true;
            this.waitingAtIntersection = false;
            this.OnPlayerMoved();
        }

        private void OnTurned()
        {
            this.canWaitAtIntersection = false;
            this.waitingAtIntersection = false;
            this.OnPlayerTurned();
        }

        private void OnPlayerTurned()
        {
            Action<Vector3> handler = this.PlayerTurned;
            if (handler != null)
                handler(this.player.transform.forward);
        }

        private void OnPlayerMoved()
        {
            Action<Vector3> handler = this.PlayerMoved;
            if (handler != null)
                handler(this.player.transform.position);
        }

        // after turning or moving, determine if the player is infront of something interactable and show the interaction panel. 
        private void CheckFacingDirection()
        {
            if (!BattleManager.InBattle)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(this.transform.position + (this.transform.forward * .25f), this.transform.forward, out hitInfo, 1))
                {
                    GameObject hitObject = hitInfo.collider.gameObject;
                    // check for various interactable components. 
                    TreasureChest tChest = hitObject.GetComponent<TreasureChest>();
                    if (tChest != null && !tChest.IsOpened)
                    {
                        this.interactionPanel.gameObject.SetActive(true);
                        this.interactionPanel.UpdateContent(tChest);
                        return;
                    }

                    TextEvent textEvent = hitObject.GetComponent<TextEvent>();
                    if(textEvent != null && textEvent.IsSignPost && !textEvent.DoNotShow)
                    {
                        this.interactionPanel.gameObject.SetActive(true);
                        this.interactionPanel.UpdateContent(textEvent);
                        return;
                    }
                }
            }
            this.interactionPanel.gameObject.SetActive(false);
        }

        private bool CheckDirection(Vector3 direction)
        {
            return !Physics.Raycast(this.transform.position, direction, 1, 1 << LayerMask.NameToLayer("wall"));
        }

        private bool AtIntersection()
        {
            this.curNavState.Clear();
            int directions = 0;
            if (!Physics.Raycast(this.transform.position, this.transform.forward, 1, 1 << LayerMask.NameToLayer("wall")))
            {
                this.curNavState.ForwardOpen = true;
                directions++;
            }
            if (!Physics.Raycast(this.transform.position, this.transform.right, 1, 1 << LayerMask.NameToLayer("wall")))
            {
                this.curNavState.RightOpen = true;
                directions++;
            }
            if (!Physics.Raycast(this.transform.position, -this.transform.right, 1, 1 << LayerMask.NameToLayer("wall")))
            {
                this.curNavState.LeftOpen = true;
                directions++;
            }
            if (!Physics.Raycast(this.transform.position, -this.transform.forward, 1, 1 << LayerMask.NameToLayer("wall")))
            {
                this.curNavState.BackOpen = true;
                directions++;
            }

            return directions > 2;
        }

        private void Start()
        {
            this.lastInput = DateTime.Now;
            if (this.settings != null && this.settings.CurrentSettings != null)
                this.pauseAtIntersectionsEnabled = this.settings.CurrentSettings.PauseAtIntersections;
            else if (this.settings != null)
                this.settings.Loaded += settings_Loaded;
        }

        void settings_Loaded(object sender, EventArgs e)
        {
            this.pauseAtIntersectionsEnabled = this.settings.CurrentSettings.PauseAtIntersections;
            this.settings.Loaded -= settings_Loaded;
        }

        private void Update()
        {
            bool wasKeyboardUsed = this.CheckKeyboardInput();
            if (!wasKeyboardUsed)
            {
                if (this.forwardFlag)
                {
                    if (!waitForButtonUp && !waitingAtIntersection)
                        this.Forward();
                }
                else if (this.leftFlag)
                {
                    if (!waitForButtonUp && !waitingAtIntersection)
                        this.Left();
                }
                else if (this.rightFlag)
                {
                    if (!waitForButtonUp && !waitingAtIntersection)
                        this.Right();
                }
                else if (this.backFlag)
                {
                    if (!waitForButtonUp && !waitingAtIntersection)
                        this.Back();
                }
                else
                {
                    this.waitForButtonUp = false;
                    this.canWaitAtIntersection = false;
                }
            }

            if (this.pauseAtIntersectionsEnabled)
            {
                this.prevNavState = this.curNavState;
                if (this.canWaitAtIntersection && this.AtIntersection())
                {
                    if (!this.prevNavState.Equals(this.curNavState))
                        this.StartCoroutine(this.StartWaitAtIntersection());
                }
            }
        }

        private IEnumerator StartWaitAtIntersection()
        {
            this.waitingAtIntersection = true;
            yield return new WaitForSeconds(.5f);
            this.waitingAtIntersection = false;
        }

        private bool CheckKeyboardInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (!this.waitForKeyUp && !waitingAtIntersection)
                {
                    this.Forward();
                    return true;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (!this.waitForKeyUp && !waitingAtIntersection)
                {
                    this.Left();
                    return true;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!this.waitForKeyUp && !waitingAtIntersection)
                {
                    this.Right();
                    return true;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (!this.waitForKeyUp && !waitingAtIntersection)
                {
                    this.Back();
                    return true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                if (!this.waitForKeyUp)
                {
                    this.TurnLeft();
                    return true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                if (!this.waitForKeyUp)
                {
                    this.TurnRight();
                    return true;
                }
            }
            else
            {
                this.waitForKeyUp = false;
                this.canWaitAtIntersection = false;
            }
            return false;
        }

        public void SetForwardFlag(bool val)
        {
            this.forwardFlag = val;
        }

        public void SetLeftFlag(bool val)
        {
            this.leftFlag = val;
        }

        public void SetRightFlag(bool val)
        {
            this.rightFlag = val;
        }

        public void SetBackFlag(bool val)
        {
            this.backFlag = val;
        }

        public void ResetFlags()
        {
            this.backFlag = false;
            this.forwardFlag = false;
            this.leftFlag = false;
            this.rightFlag = false;

            this.waitForKeyUp = true;
            this.waitForButtonUp = true;
        }
    }
}
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DungeoneerAssets
{
    public class FloorManager : MonoBehaviour
    {
        public event Action FloorChanged;
        public bool isDemo = false;
        public static bool InTown;
        public static bool InEnding;
        public static string CurrentFloorName;

        private Floor floor;
        private GameObject stairsUp;
        private GameObject stairsDown;
        private List<Interactable> interactables;
        private Vector3 startPos;
        private Vector3 startDir;
        private Vector3 endPos;
        private Vector3 endDir;
        private List<MonsterCreationData> bosses;
        private string prevFloorName;
        private string nextFloorName;

        public string floorName;
        public string displayName;
        public double powerScaleValue;
        public GameObject wallPrefab;
        public GameObject floorPrefab;
        public GameObject treasurePrefab;
        public GameObject bossPrefab;
        public MonsterFactory monsterFactory;
        public GameObject stairsUpPrefab;
        public GameObject stairsDownPrefab;
        public GameObject signPrefab;
        public GameObject emptyTextEventPrefab;
        public BattleManager battleManager;
        public Player player;
        public AudioManager audioManager;

        // killed bosses on this floor. 
        public List<BossCompletion> BossCompletion { get; set; }
        public MessageBox msgBox;

        // contains all the primitives used to make up the dungeon floor. 
        public Transform Container;

        void Start()
        {
            this.BossCompletion = new List<BossCompletion>();
            this.interactables = new List<Interactable>();
            this.bosses = new List<MonsterCreationData>();
        }

        public void UpdateInteractableDirection()
        {
            if (this.interactables != null && this.interactables.Count > 0)
            {
                Vector3 tmpForward;
                foreach (Transform t in this.interactables.Select(x => x.Transform))
                    if (t != null)
                    {
                        tmpForward = (t.position - Camera.main.transform.position).normalized;
                        t.forward = new Vector3(tmpForward.x, 0, tmpForward.z);
                    }
            }
        }

        public void LoadFloor()
        {
            this.interactables.Clear();
            BattleManager.AllowBattle = false;
            if (!string.IsNullOrEmpty(floorName))
                this.floor = FloorLoader.LoadFloor(floorName);
            if (this.floor != null)
            {
                this.LoadFloorMetaFile();
                this.ClearContainer();

                for (int i = 0; i < this.floor.Size.x; i++)
                {
                    for (int j = 0; j < this.floor.Size.y; j++)
                    {
                        if (startPos.x == i && startPos.z == j)
                        {
                            this.CreateFloor(i, j, true, false);
                            continue;
                        }
                        TileInfo info = this.floor.GetTile(i, j);
                        switch (info.Type)
                        {
                            case TileType.Wall:
                                CreateWall(i, j);
                                break;
                            case TileType.Floor: // nothing, its clear                              
                                CreateFloor(i, j, true, false);
                                break;
                            case TileType.StairsUp: // stairs up
                                CreateStairsUp(i, j);

                                Debug.Log("stairs up: " + i + ", " + j);
                                CreateFloor(i, j, false, false);
                                this.player.ExplorationInfo.AddStairsUp(this.floorName, new Vector2(i, j));
                                break;
                            case TileType.StairsDown: // stairs down
                                CreateStairsDown(i, j);
                                Debug.Log("stairs down: " + i + ", " + j);
                                CreateFloor(i, j, false, false);
                                break;
                            case TileType.Treasure:
                                this.CreateTreasure(i, j, info.ColorData);
                                this.CreateFloor(i, j, false, false);
                                break;
                            case TileType.Boss:
                                Location bossLocation = new Location { FloorName = this.floorName, X = i, Y = j };
                                if (this.BossCompletion == null || !this.BossCompletion.Any(x => x.Defeated && x.Location.Equals(bossLocation)))
                                {
                                    this.CreateFloor(i, j, false, true);
                                    this.CreateBoss(i, j);
                                }
                                else this.CreateFloor(i, j, true, false);
                                break;
                            case TileType.TextEvent:
                                this.CreateTextEvent(i, j, info.ColorData);
                                this.CreateFloor(i, j, false, false);                                
                                break;
                        }
                    }
                }
            }
            else
                throw new InvalidOperationException("floor not loaded from resource " + floorName);
        }

        private void LoadFloorMetaFile()
        {
            TextAsset txt = Resources.Load<TextAsset>(this.floorName);
            if (txt != null)
            {
                this.bosses.Clear();
                this.monsterFactory.AvailableTypes.Clear();
                LoadXMLMeta(txt.text);
            }
        }

        private void LoadXMLMeta(string xml)
        {
            bool xpModFound = false;
            XDocument xDoc = XDocument.Parse(xml);
            if (xDoc != null)
            {
                foreach (XElement element in xDoc.Element("Data").Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "StartPos":
                            Vector2 v = Vector2Helper.Parse(element.Value);
                            this.startPos = new Vector3(v.x, 0.5f, v.y);
                            break;

                        case "StartDir":
                            Vector2 v2 = Vector2Helper.Parse(element.Value);
                            this.startDir = new Vector3(v2.x, 0, v2.y);
                            break;

                        case "PrevFloor":
                            this.prevFloorName = element.Value;
                            break;

                        case "NextFloor":
                            this.nextFloorName = element.Value;
                            break;

                        case "EndPos":
                            Vector2 v3 = Vector2Helper.Parse(element.Value);
                            this.endPos = new Vector3(v3.x, 0.5f, v3.y);
                            break;

                        case "EndDir":
                            Vector2 v4 = Vector2Helper.Parse(element.Value);
                            this.endDir = new Vector3(v4.x, 0, v4.y);
                            break;

                        case "MonsterAvailability":
                            foreach (XElement avail in element.Elements())
                            {
                                MonsterAvailability mAvail = new MonsterAvailability
                                {
                                    Type = (MonsterType)Enum.Parse(typeof(MonsterType), avail.Element("Type").Value),
                                    EncounterChance = float.Parse(avail.Element("Rate").Value)
                                };

                                foreach (XElement spriteElem in avail.Element("Sprites").Elements())
                                {
                                    mAvail.Sprites.Add(new MonsterSprite
                                    {
                                        Name = spriteElem.Element("Name").Value,
                                        DisplayName = spriteElem.Element("DisplayName").Value
                                    });
                                }
                                this.monsterFactory.AvailableTypes.Add(mAvail);
                            }
                            break;

                        case "PowerScale":
                            this.powerScaleValue = double.Parse(element.Value);
                            break;
                        case "Boss":
                            this.bosses.Add(new MonsterCreationData
                            {
                                Sprite = new MonsterSprite
                                {
                                    Name = element.Element("Sprite").Element("Name").Value,
                                    DisplayName = element.Element("Sprite").Element("DisplayName").Value
                                },
                                Type = (MonsterType)Enum.Parse(typeof(MonsterType), element.Element("Type").Value),
                                PowerScale = double.Parse(element.Element("PowerScale").Value),
                                Location = new Location
                                {
                                    FloorName = this.floorName,
                                    X = int.Parse(element.Element("Location").Attribute("X").Value),
                                    Y = int.Parse(element.Element("Location").Attribute("Y").Value)
                                },
                                XP = element.HasElement("XP") ? int.Parse(element.Element("XP").Value) : (int?)null,
                                Gold = element.HasElement("Gold") ? int.Parse(element.Element("Gold").Value) : (int?)null,
                            });
                            break;
                        case "DisplayName":
                            this.displayName = element.Value;
                            break;

                        case "FightChance":
                            this.battleManager.SetFightChance(float.Parse(element.Value));
                            break;
                        case "XPModifer":
                            this.battleManager.SetXPModifer(int.Parse(element.Value));
                            xpModFound = true;
                            break;
                    }
                }

            }

            if (!xpModFound)
                this.battleManager.SetXPModifer(0);
        }

        private void CreateStairsDown(int i, int j)
        {
            this.stairsDown = (GameObject)GameObject.Instantiate(this.stairsDownPrefab);
            if (this.stairsDown.GetComponent<Collider>() != null)
                this.stairsDown.GetComponent<Collider>().isTrigger = true;
            StairsCollisionHandler colHandler = this.stairsDown.AddComponent<StairsCollisionHandler>();
            if (colHandler != null)
                colHandler.tileType = TileType.StairsDown;
            this.stairsDown.transform.position = new Vector3(i, 0, j);
            this.stairsDown.transform.forward = this.endDir;
            this.stairsDown.name = string.Format("stairsdown_{0}_{1}", i, j);
            this.stairsDown.transform.parent = this.Container;
        }

        private void CreateStairsUp(int i, int j)
        {
            this.stairsUp = (GameObject)GameObject.Instantiate(this.stairsUpPrefab);
            if (this.stairsUp.GetComponent<Collider>() != null)
                this.stairsUp.GetComponent<Collider>().isTrigger = true;
            StairsCollisionHandler colHandler = this.stairsUp.AddComponent<StairsCollisionHandler>();
            if (colHandler != null)
                colHandler.tileType = TileType.StairsUp;
            this.stairsUp.transform.position = new Vector3(i, 0, j);
            this.stairsUp.transform.forward = this.startDir;
            this.stairsUp.name = string.Format("stairsup_{0}_{1}", i, j);
            this.stairsUp.transform.parent = this.Container;
        }

        private void CreateWall(int i, int j)
        {
            GameObject go = GameObject.Instantiate(this.wallPrefab) as GameObject;
            go.transform.position = new Vector3(i, 0.5f, j);
            go.name = string.Format("wall_{0}_{1}", i, j);
            go.transform.parent = this.Container;
        }

        private void CreateTextEvent(int i, int j, Color32 color)
        {
            // red color code represents the text definition
            if(TextEventDefinitions.textEvents.ContainsKey(color.r))
            {
                TextEventDefinition def = TextEventDefinitions.textEvents[color.r];
                if(def != null)
                {
                    GameObject instance = (GameObject)GameObject.Instantiate(def.ShowSignpost ? this.signPrefab : this.emptyTextEventPrefab);
                    instance.transform.position = new Vector3(i, 0.375f, j);
                    instance.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    instance.transform.parent = this.Container;
                    instance.name = string.Format("textevent_{0}_{1}", i, j);                    
                    TextEvent textEvent = instance.GetComponent<TextEvent>();
                    textEvent.Description = def.ReadPromptText;
                    textEvent.TextLines = def.Text;
                    textEvent.Location = new Location { FloorName = this.floorName, X = i, Y = j };
                    textEvent.IsSignPost = def.ShowSignpost;
                    textEvent.ViewOnce = def.ViewOnce;
                    textEvent.Teleport = def.Teleport;
                    textEvent.TeleportTarget = def.TeleportTarget;
                    if (!string.IsNullOrEmpty(def.PromptApprovalText))
                        textEvent.YesButtonText = def.PromptApprovalText;

                    // this updates to always face the player. 
                    this.interactables.Add(new Interactable { Transform = instance.transform, ID = "TextEvent" });
                    if (!def.ShowSignpost && def.ViewOnce && this.player.FinishedInteractions.Any(x => x.Type == InteractionType.TextEvent && x.Location.Equals(textEvent.Location)))
                        textEvent.DoNotShow = true; 
                    

                }
                else Debug.LogErrorFormat("No text event for index {0} at pixel ({1}, {2})", color.r, i, j);
            }
            else Debug.LogErrorFormat("No text event for index {0} at pixel ({1}, {2})", color.r, i, j);
        }

        private void CreateTreasure(int i, int j, Color32 color)
        {
            GameObject go = GameObject.Instantiate(this.treasurePrefab) as GameObject;
            // need to assume that textures will be set the same size and will have the bottom edge of the box at the bottom of the texture. 
            go.transform.position = new Vector3(i, 0.375f, j);
            go.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            go.transform.parent = this.Container;
            go.name = string.Format("treasure_{0}_{1}", i, j);
            TreasureChest tChest = go.GetComponent<TreasureChest>();
            if (tChest != null)
            {
                // determine the contents by the color value...                
                TreasureDefinition treasureDef = TreasureDefinitions.Treasures[color.b];
                if (treasureDef != null)
                {
                    if (treasureDef.Name != "Empty")
                    {
                        switch (treasureDef.Type)
                        {
                            case DefinitionType.Item:
                                tChest.Contents = ItemDefinitions.Items[treasureDef.Name];
                                break;
                            case DefinitionType.Equipment:
                                tChest.Contents = EquipmentDefinitions.Equipment[treasureDef.Name];
                                break;
                            case DefinitionType.Spell:
                                tChest.Contents = SpellDefinitions.Spells[treasureDef.Name];
                                break;
                            case DefinitionType.Trap:
                                break;
                            case DefinitionType.Gold:
                                tChest.Contents = new ItemDefinition { Name = "Gold", DisplayName = "Gold" };
                                break;
                            default:
                                break;
                        }

                        tChest.Quantity = treasureDef.Quantity;
                        tChest.Location = new Location { FloorName = this.floorName, X = i, Y = j };
                    }
                    if (this.player.FinishedInteractions.Any(x => x.Type == InteractionType.Treasure && x.Location.Equals(tChest.Location)))
                        tChest.SetIsOpened();
                    tChest.audioManager = this.audioManager;
                }
            }
            this.interactables.Add(new Interactable { Transform = go.transform, ID = "Treasure" });
        }

        private void CreateBoss(int i, int j)
        {
            GameObject go = GameObject.Instantiate(this.bossPrefab) as GameObject;
            go.transform.position = new Vector3(i, 0.375f, j);
            go.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            go.transform.parent = this.Container;
            go.name = string.Format("boss_{0}_{1}", i, j);
            Material material = new Material(go.GetComponent<Renderer>().sharedMaterial);
            MonsterCreationData bossData = this.bosses.FirstOrDefault(x => x.Location.X == i && x.Location.Y == j);
            Sprite sp = this.monsterFactory.GetSpriteFromRepository(bossData.Sprite);
            if (sp != null)
                material.SetTexture("_MainTex", sp.texture);
            go.GetComponent<Renderer>().sharedMaterial = material;
            this.interactables.Add(new Interactable { Transform = go.transform, ID = "FloorBoss" + i + j });
        }

        private void CreateFloor(int i, int j, bool canStartFight, bool bossFight)
        {
            GameObject go = GameObject.Instantiate(this.floorPrefab) as GameObject;
            go.name = string.Format("col_{0}_{1}", i, j);
            if (canStartFight)
                go.AddComponent<OpenFloorCollisionHandler>();
            else if (bossFight)
            {
                OpenFloorCollisionHandler col = go.AddComponent<OpenFloorCollisionHandler>();
                col.BossData = this.bosses.FirstOrDefault(x => x.Location.X == i && x.Location.Y == j);

            }
            go.name = string.Format("floor_{0}_{1}", i, j);
            go.transform.position = new Vector3(i, 0, j);
            go.transform.parent = this.Container;
        }

        public void ClearContainer()
        {
            if (this.Container != null && this.Container.childCount > 0)
            {
                this.stairsDown = null;
                this.stairsUp = null;
                for (int i = this.Container.childCount - 1; i >= 0; i--)
                    GameObject.Destroy(this.Container.GetChild(i).gameObject);
            }
        }

        private void DemoEnd()
        {
            if (!msgBox.gameObject.activeInHierarchy)
            {
                PlayerNavigation nav = Camera.main.GetComponent<PlayerNavigation>();
                if (nav != null)
                {
                    nav.ResetFlags();
                    nav.enabled = false;
                }

                msgBox.Show("Thank you for playing the demo version of Dungeoneer. If you enjoyed it please rate and buy the full version.", "OK", null, () => this.GoToTown(), null);
            }
        }
        public void GoToNextFloor()
        {
            if (this.nextFloorName == "Town")
            {
                if (this.isDemo)
                    this.DemoEnd();
                else
                    this.GoToTown();
            }
            else if (this.nextFloorName == "Ending")
                this.GoToEnding();
            else
            {
                this.player.ExplorationInfo.AddStairsDown(this.floorName, new Vector2(this.stairsDown.transform.position.x, this.stairsDown.transform.position.z));
                this.floorName = this.nextFloorName;

                CurrentFloorName = floorName;

                Camera.main.GetComponent<Collider>().enabled = false;
                try
                {
                    this.LoadFloor();
                    this.SetPlayerPosition(true);
                }
                finally
                {
                    Camera.main.GetComponent<Collider>().enabled = true;
                }
                this.OnFloorChanged();
                InTown = false;
                InEnding = false;
            }
        }

        private void GoToEnding()
        {
            this.audioManager.StopBGM();
            this.audioManager.StopSFX();
            this.floorName = "Ending";
            CurrentFloorName = this.floorName;
            this.ClearContainer();
            UIManager manager = this.GetComponent<UIManager>();
            if (manager != null)
                manager.OverrideState(UIManager.UIState.EndingScene);

            PlayerNavigation nav = Camera.main.GetComponent<PlayerNavigation>();
            if (nav != null)
            {
                nav.ResetFlags();
                nav.enabled = false;
            }

            this.audioManager.PlayBGM(BGM.Ending);
            InEnding = true;
            InTown = false;
        }

        public void GoToPrevFloor()
        {
            if (this.prevFloorName == "Town")
                this.GoToTown();
            else
            {
                this.floorName = this.prevFloorName;
                CurrentFloorName = this.floorName;
                Camera.main.GetComponent<Collider>().enabled = false;
                try
                {
                    // if the floor fails to load then an exception is thrown from here and the remaining code doesn't execute. 
                    this.LoadFloor();
                    this.SetPlayerPosition(false);
                }
                finally
                {
                    Camera.main.GetComponent<Collider>().enabled = true;
                }
                this.OnFloorChanged();
                InTown = false;
            }
        }

        public void GoToTown()
        {
            this.audioManager.StopBGM();
            this.audioManager.StopSFX();
            this.floorName = "Town";
            CurrentFloorName = this.floorName;
            this.ClearContainer();
            UIManager manager = this.GetComponent<UIManager>();
            if (manager != null)
                manager.OverrideState(UIManager.UIState.Town);

            PlayerNavigation nav = Camera.main.GetComponent<PlayerNavigation>();
            if (nav != null)
            {
                nav.ResetFlags();
                nav.enabled = false;
            }

            this.audioManager.PlayBGM(BGM.Town);
            InTown = true;
            InEnding = false;
        }

        public void GoToFloor(string floorName)
        {
            if (floorName == "Town")
                this.GoToTown();
            else
            {
                UIManager manager = this.GetComponent<UIManager>();
                if (manager != null)
                    manager.OverrideState(UIManager.UIState.Dungeon);

                this.floorName = floorName;

                CurrentFloorName = this.floorName;
                this.LoadFloor();
                this.SetPlayerPosition(true);
                this.audioManager.PlayBGM(BGM.Dungeon);
                this.OnFloorChanged();
                InTown = false;
                InEnding = false;
            }
        }

        public void SetPlayerPosition(bool start)
        {
            // floor loaded. set the player position to the stairs up position            
            if (start)
            {
                Camera.main.transform.position = this.startPos;
                Camera.main.transform.forward = this.startDir;
            }
            else
            {
                Camera.main.transform.position = this.endPos;
                Camera.main.transform.forward = this.endDir;
            }
            this.UpdateInteractableDirection();
            this.player.ExplorationInfo.AddExploredSpace(this.floorName, new Vector2(this.player.transform.position.x, this.player.transform.position.z));
            PlayerNavigation nav = Camera.main.GetComponent<PlayerNavigation>();
            if (nav != null)
            {
                nav.ResetFlags();
                nav.enabled = true;
            }
        }

        private void OnFloorChanged()
        {
            Action handler = this.FloorChanged;
            if (handler != null)
                handler();
        }

        public void HideInteractables()
        {
            this.interactables.ForEach(x => x.Transform.gameObject.SetActive(false));
        }

        public void ShowInteractables()
        {
            this.interactables.ForEach(x => x.Transform.gameObject.SetActive(true));
        }

        internal void RemoveInteractables(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID");

            this.interactables.RemoveAll(x => x.ID == id);
        }

        private class Interactable
        {
            public Transform Transform { get; set; }
            public string ID { get; set; }
        }
    }
}
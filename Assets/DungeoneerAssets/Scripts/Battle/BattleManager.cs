using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DungeoneerAssets
{
    public class BattleManager : MonoBehaviour
    {
        private MonsterBase enemy;
        private bool levelGained = false;
        public float waitForEnemyTurn = 1.0f;
        public int stepsSinceLastFight;
        public int stepsToResetFightChance = 5;
        public Player player;
        public FloorManager floorManager;
        public UIManager uiManager;
        public NotificationBox notificationBox;
        public Text txtEnemyName;
        public Image enemyImage;
        public float startingFightChance = 0.1f;
        public float fightChance = 0.1f;
        public static bool InBattle { get; private set; }
        public static bool InBossBattle { get; private set; }
        public static bool AllowBattle;
        public static bool IsGameOver { get; private set; }
        public int XPModifer { get; set; }
        public AudioManager audioManager;
        public EffectManager fxManager;
        public MonsterFactory monsterFactory;
        public MonsterCreationData BossData { get; set; }
        public ScanResultsDisplay scanResultsDisplay;
        public SpellManager spellManager;
        public ItemManager itemManager;
        public MonsterBase Enemy { get { return this.enemy; } }
        public PlayerNavigation playerNavigation;
        public Button btnPlayerRun;
        public PlayerEffectIndicators playerEffectIndicators;

        private void Start()
        {
            this.fightChance = this.startingFightChance;
            this.player.LevelUp += player_LevelUp;
            this.playerNavigation.PlayerMoved += playerNavigation_PlayerMoved;
        }

        void playerNavigation_PlayerMoved(Vector3 obj)
        {
            this.stepsSinceLastFight++;
            if (this.fightChance < this.startingFightChance && this.stepsSinceLastFight >= this.stepsToResetFightChance)
            {
                this.fightChance *= 2;
                if (this.fightChance > this.startingFightChance)
                    this.fightChance = this.startingFightChance;
            }
        }

        private void OnDestroy()
        {
            this.player.LevelUp -= player_LevelUp;
            this.playerNavigation.PlayerMoved -= playerNavigation_PlayerMoved;
        }

        void player_LevelUp(object sender, System.EventArgs e)
        {
            levelGained = true;
        }

        public void RollForBattle()
        {
            if (AllowBattle)
            {
                float val = UnityEngine.Random.Range(0f, 1.0f);
                if (val <= this.fightChance)
                    this.StartBattle();
            }
        }

        internal void StartBossBattle()
        {
            if (this.BossData != null)
            {
                if (this.floorManager.BossCompletion == null || !this.floorManager.BossCompletion.Any(x => x.Defeated && x.Location.Equals(this.BossData.Location)))
                    this.StartBattle();
            }
        }

        public void StartBattle()
        {
            BattleManager.IsGameOver = false;
            BattleManager.InBattle = true;
            this.stepsSinceLastFight = 0;
            this.fightChance /= 2;
            this.floorManager.HideInteractables();
            // disable navigation - keyboard active when ui was disabled.             
            this.playerNavigation.ResetFlags();
            this.playerNavigation.enabled = false;

            this.levelGained = false;
            this.player.IsFocused = false;
            // swap ui            
            uiManager.OverrideState(UIManager.UIState.Battle);

            notificationBox.Show();
            notificationBox.ClearText();
            this.scanResultsDisplay.Show(false);
            // determine and create enemy
            if (this.BossData != null)
            {
                this.CreateBoss();
                this.audioManager.PlayBGM(BGM.Boss);
                BattleManager.InBossBattle = true;
                this.btnPlayerRun.interactable = false;
                this.btnPlayerRun.UpdateTextColor(this.btnPlayerRun.colors.disabledColor);
            }
            else
            {
                this.CreateEnemy();
                this.audioManager.PlayBGM(BGM.Fight);
                this.btnPlayerRun.interactable = true;
                this.btnPlayerRun.UpdateTextColor(Color.white);
            }

            notificationBox.AddText("Encountered an enemy!");

            // determine who gets first attack round
            if (player.Agi > this.enemy.Stats.Agi)
                uiManager.BattleCommands(true);
            else
            {
                uiManager.BattleCommands(false);
                Invoke("EnemyTurn", waitForEnemyTurn);
            }
        }

        private IEnumerator WonBattle()
        {
            yield return new WaitForSeconds(1f);  // wait so the player can see the final message

            notificationBox.ClearText();  // will remove the clear and add a new line instead once I get the scrolling text working. 
            notificationBox.AddText("The enemy has been defeated");

            // invoke after action report            
            this.fxManager.StopEffects(Effect.BarrierSpell, Effect.ShieldSpell, Effect.EnemyBarrierSpell, Effect.EnemyShieldSpell);
            this.scanResultsDisplay.Show(false);
            this.audioManager.StopBGM();
            if (this.BossData != null)
                this.audioManager.PlaySFX(SFX.BossFightWon);
            else this.audioManager.PlaySFX(SFX.NormalFightWon);

            yield return new WaitForSeconds(0.5f);

            int xp = this.enemy.XP;
            int gold = this.enemy.Gold;
            this.DestroyEnemy();
            this.player.AddXP(xp + this.XPModifer);
            this.player.AddGold(gold);
            notificationBox.AddText("Gained " + xp + " XP");
            notificationBox.AddText("Gained " + gold + " Gold");

            if (this.BossData != null)
            {
                this.floorManager.BossCompletion.Add(new BossCompletion { Defeated = true, Location = this.BossData.Location });
                this.floorManager.RemoveInteractables("FloorBoss" + this.BossData.Location.X + this.BossData.Location.Y);
                this.BossData = null;
            }
            yield return new WaitForSeconds(2.0f);
            if (this.levelGained)
            {
                this.ReturnToNavigation();
                this.uiManager.PushState(UIManager.UIState.LevelUp);
            }
            else
                this.ReturnToNavigation();
        }

        // this is the end all function for resetting battle state. every end condition calls this
        private void ReturnToNavigation()
        {
            this.playerNavigation.enabled = true;
            this.playerEffectIndicators.ClearIndicators();
            this.scanResultsDisplay.SetText("");
            this.scanResultsDisplay.Show(false);
            this.fxManager.StopEffects(Effect.BarrierSpell, Effect.ShieldSpell, Effect.EnemyBarrierSpell, Effect.EnemyShieldSpell);
            uiManager.OverrideState(UIManager.UIState.Dungeon);
            notificationBox.ClearText();
            notificationBox.Hide();
            if (this.player.StatMods != null && this.player.StatMods.Count > 0)
                this.player.StatMods.Clear();
            this.SetIsFocus(false);
            BattleManager.InBattle = false;
            BattleManager.InBossBattle = false;
            this.floorManager.ShowInteractables();
            this.audioManager.PlayBGM(BGM.Dungeon);
            if (this.uiManager.MapOpened)
                this.uiManager.PushState(UIManager.UIState.Map);
        }

        internal void Reset()
        {
            this.scanResultsDisplay.SetText("");
            this.fxManager.StopEffects(Effect.BarrierSpell, Effect.ShieldSpell, Effect.EnemyBarrierSpell, Effect.EnemyShieldSpell);
            if (this.player.StatMods != null && this.player.StatMods.Count > 0)
                this.player.StatMods.Clear();
            this.scanResultsDisplay.Show(false);
            this.notificationBox.ClearText();
            this.notificationBox.Hide();
            BattleManager.InBattle = false;
            BattleManager.InBossBattle = false;
            this.DestroyEnemy();
            this.playerNavigation.enabled = true;
        }

        public void SetFightChance(float f)
        {
            this.startingFightChance = f;
            this.fightChance = f;
            this.stepsSinceLastFight = 0;
        }

        internal void SetXPModifer(int xpMod)
        {
            this.XPModifer = xpMod;
        }

        private void DestroyEnemy()
        {
            this.enemyImage.gameObject.SetActive(false);
            this.enemy = null;
        }

        private void CreateEnemy()
        {
            FloorManager fm = this.GetComponent<FloorManager>();
            double scale = fm.powerScaleValue;
            enemyImage.gameObject.SetActive(true);
            this.enemy = this.monsterFactory.Create(this.enemyImage.gameObject, scale);

            txtEnemyName.text = this.enemy.MonsterName;
        }

        private void CreateBoss()
        {
            this.enemyImage.gameObject.SetActive(true);
            this.enemy = this.monsterFactory.Create(this.enemyImage.gameObject, this.BossData);
            txtEnemyName.text = this.enemy.MonsterName;
        }

        #region PlayerActions



        public void PlayerAttack()
        {
            notificationBox.ClearText();
            uiManager.BattleCommands(false);

            this.ExecutePlayerAttack();
            if (this.enemy.Stats.HP > 0)
                Invoke("EnemyTurn", waitForEnemyTurn);
            else if (this.enemy.Stats.HP <= 0)
                this.StartCoroutine(this.WonBattle());
        }

        public IEnumerator PlayerDoubleAttack()
        {
            notificationBox.ClearText();
            uiManager.BattleCommands(false);

            this.ExecutePlayerAttack();
            yield return new WaitForSeconds(.4f);
            this.ExecutePlayerAttack();

            if (this.enemy != null && this.enemy.Stats.HP > 0)
                Invoke("EnemyTurn", waitForEnemyTurn);
            else if (this.enemy.Stats.HP <= 0)
                this.StartCoroutine(this.WonBattle());

            if (!InBattle)
                this.StartCoroutine(notificationBox.DelayHide(2));
        }

        private void ExecutePlayerAttack()
        {

            this.fxManager.ShowEffect(Effect.PlayerAttack);
            if (this.enemy.Stats.Agi > this.player.Agi)
            {
                DodgeCalculator calc = new DodgeCalculator();
                calc.PlayerAgility = this.player.Agi;
                calc.EnemyAgility = this.enemy.Stats.Agi;
                calc.CalculateForEnemy();
                if (calc.EnemyDodged)
                {
                    this.audioManager.PlaySFX(SFX.PlayerMiss);
                    this.notificationBox.AddText("Player attack missed.");
                    return;
                }
            }
            this.enemy.PlayerUsesPhysicalAttacks = true;
            this.audioManager.PlaySFX(SFX.PlayerAttack);

            int damage = 0;
            EquipmentSlot slot = this.player.CurrentInventory.GetEquipedItem(EquipmentType.Weapon);
            if (slot != null && slot.Definition.Value.MagicalAttack > 0 && this.player.MP > 2)
            {
                int magAttack = this.player.MagicalAttack;
                magAttack = this.CalculateFinalDamage(magAttack, AttackType.Magical);
                damage += magAttack;
                this.player.RestoreMP(-2);
            }
            if (slot != null && slot.Definition.Value.PhysicalAttack > 0)
            {
                int physDamage = this.player.PhysicalAttack;
                physDamage = this.CalculateFinalDamage(physDamage, AttackType.Physical);
                damage += physDamage;
            }
            notificationBox.AddText(string.Format("Player attacks for {0} damage.", damage));
            this.DamageEnemy(damage);
        }

        private int CalculatePlayerAttackDamage()
        {
            int damage = 0;
            EquipmentSlot slot = this.player.CurrentInventory.GetEquipedItem(EquipmentType.Weapon);
            if (slot != null && slot.Definition.Value.MagicalAttack > 0 && this.player.MP > 2)
            {
                int magAttack = this.player.MagicalAttack;
                magAttack = this.CalculateFinalDamage(magAttack, AttackType.Magical);
                damage += magAttack;
            }
            if (slot != null && slot.Definition.Value.PhysicalAttack > 0)
            {
                int physDamage = this.player.PhysicalAttack;
                physDamage = this.CalculateFinalDamage(physDamage, AttackType.Physical);
                damage += physDamage;
            }

            return damage;
        }

        public void PlayerSpell(string spellName)
        {
            notificationBox.ClearText();
            notificationBox.Show();

            if (InBattle)
            {
                uiManager.PopState();
                uiManager.BattleCommands(false);
            }
            SpellDefinition spellDef = SpellDefinitions.Spells[spellName];
            this.notificationBox.AddText(string.Format("Player {0} {1}", spellDef.AttackType == AttackType.Magical ? "casts" : "performs", spellName));
            int amount = this.spellManager.CastSpell(spellName);

            switch (spellName)
            {
                case "Fire":
                    if (this.player.IsFocused)
                        amount = (int)(amount * 1.5f);
                    this.enemy.PlayerUsesMagicalAttacks = true;
                    amount = this.CalculateFinalDamage(amount, AttackType.Magical);
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.DamageEnemy(amount);
                    this.SetIsFocus(false);
                    break;
                case "Heal":
                    if (this.player.IsFocused)
                        amount = (int)(amount * 1.5f);
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.player.Heal(amount);
                    this.SetIsFocus(false);
                    break;
                case "Scan":
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.scanResultsDisplay.Show(true);
                    break;
                case "Shield":
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.player.StatMods.Add(new TimedStatModifier { AffectedStat = TimedStatModifier.Stat.PhysDefense, Amount = amount, RemainingDuration = this.player.IsFocused ? 8 : 5, StatSource = TimedStatModifier.Source.ShieldSpell });
                    this.SetIsFocus(false);
                    break;
                case "Barrier":
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.player.StatMods.Add(new TimedStatModifier { AffectedStat = TimedStatModifier.Stat.MagDefense, Amount = amount, RemainingDuration = this.player.IsFocused ? 8 : 5, StatSource = TimedStatModifier.Source.BarrierSpell });
                    this.SetIsFocus(false);
                    break;
                case "Focus":
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.SetIsFocus(true);
                    break;
                case "Meditate":
                    amount = (int)(UnityEngine.Random.Range(0.12f, 0.2f) * this.player.MaxMP);
                    if (this.player.IsFocused)
                        amount = (int)(amount * 1.5f);
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.player.RestoreMP(amount);
                    this.SetIsFocus(false);
                    break;
                case "FierceAttack":
                    amount = CalculatePlayerAttackDamage();
                    amount = (int)(amount * UnityEngine.Random.Range(1.8f, 2.2f));
                    this.enemy.PlayerUsesPhysicalAttacks = true;
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.DamageEnemy(amount);
                    break;
                case "Stun":
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    if (amount == 1)
                        this.enemy.StatMods.Add(new TimedStatModifier { AffectedStat = TimedStatModifier.Stat.Stun, Amount = 1, RemainingDuration = 3 });
                    break;
                case "DoubleAttack":
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.StartCoroutine(this.PlayerDoubleAttack());
                    return;

                case "ExtendedGuard":
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.player.StatMods.Add(new TimedStatModifier { AffectedStat = TimedStatModifier.Stat.PhysDefense, Amount = amount, RemainingDuration = 4, StatSource = TimedStatModifier.Source.ExtendedGuard });
                    this.playerEffectIndicators.AddExtendedGuard();
                    break;
                case "PerfectGuard":
                    notificationBox.AddText(this.spellManager.GetDescriptionText(spellName, amount));
                    this.player.StatMods.Add(new TimedStatModifier { AffectedStat = TimedStatModifier.Stat.PhysDefense, Amount = amount, RemainingDuration = 3, StatSource = TimedStatModifier.Source.PerfectGuard });
                    this.playerEffectIndicators.AddPerfectGuard();
                    break;
            }

            if (this.enemy != null && this.enemy.Stats.HP > 0)
                Invoke("EnemyTurn", waitForEnemyTurn);
            else if (this.enemy.Stats.HP <= 0)
                this.StartCoroutine(this.WonBattle());

            if (!InBattle)
                this.StartCoroutine(notificationBox.DelayHide(2));
        }

        private void SetIsFocus(bool v)
        {
            this.player.IsFocused = v;
            if (v)
                this.playerEffectIndicators.AddFocus();
            else
                this.playerEffectIndicators.RemoveFocus();
        }

        public void PlayerDefend()
        {
            // the point of the player defending is that if the enemy has an attack pattern thats recognizable the player can defend the round of a powerful attack
            // assuming their agility is higher than the enemies and they get the first action of each round. 
            notificationBox.ClearText();
            uiManager.BattleCommands(false);
            notificationBox.AddText("Player defends.");
            this.player.IsDefending = true;
            Invoke("EnemyTurn", waitForEnemyTurn);
        }

        public void PlayerItem(string item)
        {
            notificationBox.ClearText();

            if (InBattle)
            {
                uiManager.PopState();
                uiManager.BattleCommands(false);
            }

            notificationBox.Show();

            int amount = this.itemManager.UseItem(item);

            switch (item)
            {
                case "HealPotion":
                case "HealPotion2":
                    notificationBox.AddText("player used a Heal Potion.");
                    notificationBox.AddText(string.Format("player healed for {0} hp.", amount));
                    break;

                case "SmokeBomb":
                    this.StartCoroutine(this.UseSmokeBomb(item));
                    break;

                case "MPPotion":
                case "MPPotionX":
                    notificationBox.AddText("player used a MP Potion.");
                    notificationBox.AddText(string.Format("player restored {0} MP.", amount));
                    break;
            }

            if (item != "SmokeBomb")
                if (this.enemy != null && this.enemy.Stats.HP > 0)
                    Invoke("EnemyTurn", waitForEnemyTurn);

            if (!InBattle)
                this.StartCoroutine(notificationBox.DelayHide(2));
        }

        private IEnumerator UseSmokeBomb(string item)
        {
            this.audioManager.PlaySFX(SFX.SmokeBomb);
            bool ran = player.UseItem(item) == 1;
            notificationBox.AddText("player used a Smoke Bomb.");
            yield return new WaitForSeconds(1);
            if (ran)
            {
                this.fxManager.StopEffects(Effect.BarrierSpell, Effect.ShieldSpell, Effect.EnemyBarrierSpell, Effect.EnemyShieldSpell);
                this.scanResultsDisplay.Show(false);
                this.audioManager.PlaySFX(SFX.RunAway);
                notificationBox.AddText("Ran away.");
                this.DestroyEnemy();
                Invoke("ReturnToNavigation", 1.0f);
            }
            else
            {
                notificationBox.AddText("Smoke bomb failed.");
                if (this.enemy != null && this.enemy.Stats.HP > 0)
                    Invoke("EnemyTurn", waitForEnemyTurn);
            }
        }

        public void PlayerRun()
        {
            // 33% chance of running from battle
            notificationBox.ClearText();
            notificationBox.AddText("Player tries to run away.");

            float val = UnityEngine.Random.Range(0f, 1.0f);
            float chance = this.BossData != null ? 0.20f : 0.33f;
            if (val <= chance)
            {
                this.fxManager.StopEffects(Effect.BarrierSpell, Effect.ShieldSpell, Effect.EnemyBarrierSpell, Effect.EnemyShieldSpell);
                this.scanResultsDisplay.Show(false);
                this.audioManager.PlaySFX(SFX.RunAway);
                notificationBox.AddText("Ran away.");
                this.DestroyEnemy();
                Invoke("ReturnToNavigation", 1.0f);
            }
            else
            {
                notificationBox.AddText("Can't run.");
                Invoke("EnemyTurn", waitForEnemyTurn);
            }
        }

        public void PlayerMenu()
        {
            this.notificationBox.ClearText();
            this.EnemyTurn();
        }
        #endregion

        private void BeforeEnemyTurn()
        {
            this.enemy.OnBattleTurnFinished();
            this.ClearEnemyDuratonEffects();
        }

        public void EnemyTurn()
        {
            this.BeforeEnemyTurn();
            this.player.CalculateBattleStats();

            if (!this.enemy.StatMods.Any(x => x.AffectedStat == TimedStatModifier.Stat.Stun))
            {
                CharacterAction action = this.enemy.EnemyTurn();
                switch (action.Type)
                {
                    case CharacterActionType.Attack:
                        this.OnEnemyAttack(action);
                        break;
                    case CharacterActionType.Defend:
                        this.OnEnemyDefend(action);
                        break;
                    case CharacterActionType.Heal:
                        this.OnEnemyHeal(action);
                        break;
                    case CharacterActionType.Run:
                        this.OnEnemyRun(action);
                        break;
                    case CharacterActionType.Barrier:
                        this.OnEnemyBarrier(action);
                        break;
                    case CharacterActionType.Shield:
                        this.OnEnemyShield(action);
                        break;
                    case CharacterActionType.Information:
                        this.notificationBox.AddText(action.Description);
                        break;
                }
            }
            else
            {
                this.notificationBox.AddText("Enemy is stunned.");
            }

            this.enemy.CalculateDerivedStats();
            this.UpdateScanDisplay();
            if (this.player.HP <= 0)
            {
                // battle finished
                notificationBox.AddText("You lose.");
                StartCoroutine(GameOver());
                uiManager.BattleCommands(false);
                // wait then return to title screen or town. 
            }
            else
                this.Invoke("StartPlayerTurn", .5f);
        }

        private void UpdateScanDisplay()
        {
            if (this.scanResultsDisplay.gameObject.activeInHierarchy)
            {
                string display = string.Format("HP: {0}/{1}", this.enemy.Stats.HP > 0 ? this.enemy.Stats.HP : 0, this.enemy.Stats.MaxHP);
                if (this.enemy.Stats.MaxMP > 0)
                    display += string.Format(" MP: {0}/{1}", this.enemy.Stats.MP, this.enemy.Stats.MaxMP);

                this.scanResultsDisplay.SetText(display);
            }
        }

        private void ClearPlayerDurationEffects()
        {
            if (this.player != null && this.player.StatMods != null)
            {
                if (!this.player.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.BarrierSpell))
                    this.fxManager.StopEffect(Effect.BarrierSpell);
                if (!this.player.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.ShieldSpell))
                    this.fxManager.StopEffect(Effect.ShieldSpell);
                if (!this.player.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.ExtendedGuard))
                    this.playerEffectIndicators.RemoveExtendedGuard();
                if (!this.player.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.PerfectGuard))
                    this.playerEffectIndicators.RemovePerfectGuard();
            }
        }

        private void ClearEnemyDuratonEffects()
        {
            if (this.enemy != null && this.enemy.StatMods != null)
            {
                if (!this.enemy.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.BarrierSpell))
                    this.fxManager.StopEffect(Effect.EnemyBarrierSpell);
                if (!this.enemy.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.ShieldSpell))
                    this.fxManager.StopEffect(Effect.EnemyShieldSpell);
            }
        }

        private IEnumerator ShakeCamera()
        {
            Shake camShake = this.player.gameObject.GetComponent<Shake>();
            if (camShake != null)
            {
                camShake.enabled = true;
                yield return new WaitForSeconds(.3f);
                camShake.enabled = false;
            }
            yield return null;
        }

        private void OnEnemyAttack(CharacterAction action)
        {
            if (action.AttackType == AttackType.Physical)
            {
                int agiDiff = this.player.Agi - this.enemy.Stats.Agi;
                if (agiDiff > 0)
                {
                    DodgeCalculator calc = new DodgeCalculator();
                    calc.PlayerAgility = this.player.Agi;
                    calc.EnemyAgility = this.enemy.Stats.Agi;
                    calc.CalculateForPlayer();
                    if (calc.PlayerDodged)
                    {
                        this.audioManager.PlaySFX(SFX.MonsterMiss);
                        this.notificationBox.AddText("Enemy attack missed.");
                        return;
                    }
                }
                this.audioManager.PlaySFX(SFX.MonsterAttack);
                this.StartCoroutine(this.ShakeCamera());
            }
            else if (action.AttackType == AttackType.Magical)
            {
                this.audioManager.PlaySFX(SFX.FireSpell);
                this.fxManager.ShowEffect(Effect.EnemyFireSpell);
            }

            int damage = action.Value;
            if (this.player.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.PerfectGuard))
                damage = 0;
            else
            {
                damage -= (action.AttackType == AttackType.Physical ? this.player.PhysicalDefense : this.player.MagicalDefense);
                if (this.player.IsDefending || this.player.StatMods.Any(x => x.StatSource == TimedStatModifier.Source.ExtendedGuard))
                    damage /= 2;

                if (damage <= 0)
                    damage = 1;
            }

            notificationBox.AddText(string.Format(action.Description, damage));
            this.player.Damage(damage);
        }

        private void OnEnemyHeal(CharacterAction action)
        {
            this.audioManager.PlaySFX(SFX.HealSpell);
            this.fxManager.ShowEffect(Effect.EnemyHealSpell);
            int amount = action.Value;
            notificationBox.AddText(string.Format(action.Description, amount));
            this.enemy.Heal(amount);
        }

        private void OnEnemyBarrier(CharacterAction action)
        {
            this.audioManager.PlaySFX(SFX.ShieldSpell);
            this.fxManager.StartEffect(Effect.EnemyBarrierSpell);
            notificationBox.AddText(action.Description);
            this.enemy.StatMods.Add(new TimedStatModifier { AffectedStat = TimedStatModifier.Stat.MagDefense, Amount = action.Value, RemainingDuration = 5, StatSource = TimedStatModifier.Source.BarrierSpell });
        }

        private void OnEnemyShield(CharacterAction action)
        {
            this.audioManager.PlaySFX(SFX.ShieldSpell);
            this.fxManager.StartEffect(Effect.EnemyShieldSpell);
            notificationBox.AddText(action.Description);
            this.enemy.StatMods.Add(new TimedStatModifier { AffectedStat = TimedStatModifier.Stat.PhysDefense, Amount = action.Value, RemainingDuration = 5, StatSource = TimedStatModifier.Source.ShieldSpell });
        }

        private void OnEnemyRun(CharacterAction action)
        { }

        private void OnEnemyDefend(CharacterAction action)
        { }

        public void DamageEnemy(int damage)
        {
            this.enemy.Stats.HP -= damage;
            this.UpdateScanDisplay();
        }

        public int CalculateFinalDamage(int damage, AttackType attackType)
        {
            damage -= (attackType == AttackType.Physical ? this.enemy.PhysicalDefense : this.enemy.MagicalDefense);
            if (damage <= 0)
                damage = 1;
            return damage;
        }

        private IEnumerator GameOver()
        {
            this.fxManager.StopEffects(Effect.BarrierSpell, Effect.ShieldSpell, Effect.EnemyBarrierSpell, Effect.EnemyShieldSpell);
            this.player.StatMods.Clear();
            this.scanResultsDisplay.Show(false);
            this.audioManager.StopBGM();
            this.audioManager.PlaySFX(SFX.GameOver);
            IsGameOver = true;
            uiManager.GameOver(true);
            uiManager.BattleCommands(false);
            yield return null;
        }

        public void GameOverReturnToTown()
        {
            this.DestroyEnemy();
            this.ReturnToNavigation();
            uiManager.GameOver(false);
            notificationBox.ClearText();
            notificationBox.Hide();
            this.player.Heal(1);
            this.player.Gold -= (int)(this.player.Gold / 4);
            FloorManager fm = this.GetComponent<FloorManager>();
            fm.GoToTown();
            IsGameOver = false;
        }

        public void GameOverLoadGame()
        {
            this.audioManager.StopBGM();
            this.audioManager.StopSFX();
            this.DestroyEnemy();
            this.ReturnToNavigation();
            uiManager.GameOver(false);
            notificationBox.ClearText();
            notificationBox.Hide();
            IsGameOver = false;
        }

        private void StartPlayerTurn()
        {
            this.player.OnBattleTurnFinished();
            this.ClearPlayerDurationEffects();

            uiManager.BattleCommands(true);
            this.player.IsDefending = false;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DungeoneerAssets
{
    [Serializable]
    public class Player : MonoBehaviour, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler LevelUp;
        private int hp;
        private int mp;
        private int maxHP;
        private int maxMP;
        private int gold;
        private int level;
        private int points;

        [SerializeField]
        private bool giveAllSpells;
        [SerializeField]
        private bool giveLevel30;
        [SerializeField]
        private bool give10kGold;

        private void Start()
        {
            this.DefaultInitialize();
        }

        internal StatRequirement GetStatRequirement()
        {
            return new StatRequirement { Agi = this.Agi, Int = this.Int, Str = this.Str, Vit = this.Vit };
        }

        public void CalculateBattleStats()
        {
            this.PhysicalAttack = this.Str;
            this.MagicalAttack = this.Int;
            this.PhysicalDefense = (int)(this.Vit * .5f);
            this.MagicalDefense = (int)(this.Int * .5f) + (int)(this.Vit * .2f);

            foreach (var item in this.CurrentInventory.Equipment.Where(x => x.IsEquipped))
            {
                this.PhysicalAttack += item.Definition.Value.PhysicalAttack;
                this.PhysicalDefense += item.Definition.Value.PhysicalDefense;
                this.MagicalAttack += item.Definition.Value.MagicalAttack;
                this.MagicalDefense += item.Definition.Value.MagicalDefense;
                this.Str += item.Definition.Value.Str;
                this.Vit += item.Definition.Value.Vit;
                this.Int += item.Definition.Value.Int;
                this.Agi += item.Definition.Value.Agi;
                this.MaxHP += item.Definition.Value.MaxHP;
                this.MaxMP += item.Definition.Value.MaxMP;
            }

            if (this.StatMods != null)
            {
                foreach (TimedStatModifier mod in this.StatMods)
                {
                    switch (mod.AffectedStat)
                    {
                        case TimedStatModifier.Stat.PhysAttack:
                            this.PhysicalAttack += mod.Amount;
                            break;
                        case TimedStatModifier.Stat.PhysDefense:
                            this.PhysicalDefense += mod.Amount;
                            break;
                        case TimedStatModifier.Stat.MagAttack:
                            this.MagicalAttack += mod.Amount;
                            break;
                        case TimedStatModifier.Stat.MagDefense:
                            this.MagicalDefense += mod.Amount;
                            break;
                    }
                }
            }
        }

        // temp until the save game mechanism is in place. 
        public void DefaultInitialize()
        {
            this.Level = 1;
            this.HP = 20;
            this.MaxHP = 20;
            this.MP = 15;
            this.MaxMP = 15;
            this.Str = 4;
            this.Vit = 4;
            this.Agi = 4;
            this.Int = 4;
            this.Gold = 200;
            this.CurrentXP = 0;
            this.IsDefending = false;
            this.XPToNextLevel = LevelUpDefinitions.XPPerLevel[this.Level];
            this.Points = 0;
            this.CurrentInventory = new Inventory();
            this.CurrentInventory.AddItem("HealPotion", 5);
            this.CurrentInventory.AddItem("SmokeBomb", 3);

            this.CurrentInventory.AddEquipment("WoodSword");
            this.CurrentInventory.AddEquipment("PlainClothes");
            this.CurrentInventory.EquipItem(this.CurrentInventory.Equipment.FirstOrDefault(x => x.Name == "WoodSword"));
            this.CurrentInventory.EquipItem(this.CurrentInventory.Equipment.FirstOrDefault(x => x.Name == "PlainClothes"));

            this.Spells = new List<SpellDefinition>();
            this.FinishedInteractions = new List<Interaction>();
            this.ExplorationInfo = new ExplorationData();
            this.StatMods = new List<TimedStatModifier>();
            this.CalculateBattleStats();
            this.Spells.Add(SpellDefinitions.GetSpell("Heal"));

            if (this.giveAllSpells)
            {
                this.Spells.Clear();
                foreach (SpellDefinition spell in SpellDefinitions.Spells.Values)
                {
                    this.Spells.Add(spell);
                }
            }

            if (this.give10kGold)
                this.AddGold(10000);

            if (this.giveLevel30)
                for (int i = 0; i < 30; i++)
                    this.AddXP(100000);
        }

        public void Damage(int val)
        {
            this.HP -= val;
        }

        public void AddXP(int xp)
        {
            this.CurrentXP += xp;
            if (this.CurrentXP >= this.XPToNextLevel)
            {
                this.Level += 1;
                this.XPToNextLevel = LevelUpDefinitions.XPPerLevel[this.Level];
                this.CurrentXP = 0;
                this.Points += 1;
                this.MaxHP += this.Vit + (int)(this.Vit * UnityEngine.Random.Range(.2f, .4f));
                this.MaxMP += this.Int + (int)(this.Int * UnityEngine.Random.Range(.2f, .4f));
                this.HP = this.MaxHP;
                this.MP = this.MaxMP;
                this.OnLevelUp();
            }
        }

        public void AddGold(int gold)
        {
            this.Gold += gold;
        }

        public void RestoreToFull()
        {
            this.HP = this.MaxHP;
            this.MP = this.MaxMP;
        }

        public void Heal(int amount)
        {
            int newHP = 0;
            if (this.hp <= 0)
                newHP = amount;
            else
                newHP = this.hp + amount;

            if (newHP > this.MaxHP)
                newHP = this.MaxHP;
            this.HP = newHP;
        }

        public void RestoreMP(int amount)
        {
            int newMP = this.mp + amount;
            if (newMP > this.MaxMP)
                newMP = this.MaxMP;
            this.MP = newMP;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(name));
        }

        protected void OnLevelUp()
        {
            EventHandler h = this.LevelUp;
            if (h != null)
                h(this, null);
        }

        public bool CheckMPRequirement(string spellName)
        {
            SpellDefinition sp = SpellDefinitions.GetSpell(spellName);
            if (sp != null)
                return this.MP >= sp.MPCost;
            else
                throw new InvalidOperationException("spell " + spellName + " not found.");
        }

        /// <summary>
        /// return calculated final power of spell and decrement MP from player should be preceded by CanCastSpell
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int CastSpell(string name)
        {
            SpellDefinition sp = SpellDefinitions.GetSpell(name);
            if (sp != null)
            {
                this.MP -= sp.MPCost;
                return sp.CalculatePower(this.Int);
            }
            else
                throw new InvalidOperationException("spell " + name + " not found.");
        }

        internal int UseItem(string item)
        {
            var playerItem = CurrentInventory.Items.FirstOrDefault(x => x.Name == item);
            if (playerItem.Quantity > 0)
            {
                playerItem.Quantity -= 1;
                if (playerItem.Quantity == 0)
                    this.CurrentInventory.Items.Remove(playerItem);
                return playerItem.Definition.CalculatedEffect();
            }
            return 0;
        }

        internal void HydrateStats(PlayerStats stats)
        {
            this.HP = stats.HP;
            this.MP = stats.MP;
            this.MaxHP = stats.MaxHP;
            this.MaxMP = stats.MaxMP;
            this.Level = stats.Level;
            this.Gold = stats.Gold;
            this.Str = stats.Str;
            this.Vit = stats.Vit;
            this.Int = stats.Int;
            this.Agi = stats.Agi;
            this.CurrentXP = stats.CurrentXP;
            this.Points = stats.Points;
            this.XPToNextLevel = LevelUpDefinitions.XPPerLevel[this.Level];
        }

        internal void HydrateSpells(List<string> list)
        {
            if (list != null && list.Count > 0)
            {
                this.Spells = new List<SpellDefinition>();
                foreach (string spell in list)
                    this.Spells.Add(SpellDefinitions.GetSpell(spell));
            }
        }

        internal bool CanUseItem(string itemName)
        {
            if (this.CurrentInventory.Items != null)
            {
                ItemSlot item = this.CurrentInventory.Items.FirstOrDefault(x => x.Name == itemName);
                if (item != null)
                    return item.Definition.CanUse(this);
            }
            return false;
        }

        public bool CanCastSpell(string spellName)
        {
            SpellDefinition spell = this.Spells.FirstOrDefault(x => x.Name == spellName);
            if (spell != null)
                return spell.CanUse(this);
            return false;
        }

        public void OnBattleTurnFinished()
        {
            if (this.StatMods != null && this.StatMods.Count > 0)
            {
                this.StatMods.ForEach(x => x.RemainingDuration -= 1);
                this.StatMods.RemoveAll(x => x.RemainingDuration <= 0);
                this.CalculateBattleStats();
            }
        }


        public int Level
        {
            get
            {
                return this.level;
            }
            private set
            {
                this.level = value;
                OnPropertyChanged("Level");
            }
        }

        public int HP
        {
            get
            {
                return this.hp;
            }
            private set
            {
                this.hp = value;
                OnPropertyChanged("HP");
            }
        }

        public int MP
        {
            get
            {
                return this.mp;
            }
            private set
            {
                this.mp = value;
                OnPropertyChanged("MP");
            }
        }

        public int MaxHP
        {
            get
            {
                return this.maxHP;
            }
            private set
            {
                this.maxHP = value;
                OnPropertyChanged("MaxHP");
            }
        }

        public int MaxMP
        {
            get
            {
                return this.maxMP;
            }
            private set
            {
                this.maxMP = value;
                OnPropertyChanged("MaxMP");
            }
        }

        public int Agi { get; set; }
        public int Str { get; set; }
        public int Int { get; set; }
        public int Vit { get; set; }
        public int CurrentXP { get; set; }
        public int XPToNextLevel { get; set; }
        public int Points { get { return points; } set { this.points = value; OnPropertyChanged("Points"); } }
        public int Gold
        {
            get
            {
                return this.gold;
            }
            set
            {
                this.gold = value;
                OnPropertyChanged("Gold");
            }
        }
        public bool IsDefending { get; set; }

        public int PhysicalAttack { get; private set; }
        public int PhysicalDefense { get; private set; }
        public int MagicalAttack { get; private set; }
        public int MagicalDefense { get; private set; }

        public Inventory CurrentInventory { get; set; }
        public List<SpellDefinition> Spells { get; set; }
        public List<TimedStatModifier> StatMods { get; private set; }
        public List<Interaction> FinishedInteractions { get; set; }
        public ExplorationData ExplorationInfo { get; set; }
        public bool IsFocused { get; set; }
    }
}
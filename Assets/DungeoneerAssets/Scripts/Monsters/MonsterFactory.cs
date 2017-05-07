using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class MonsterFactory : MonoBehaviour
    {
        public MonsterImageRepository imageRepository;
        public List<MonsterAvailability> AvailableTypes { get; set; }

        public MonsterFactory()
        {
            AvailableTypes = new List<MonsterAvailability>();
        }

        public MonsterBase Create(GameObject parent, double powerScale)
        {
            if (AvailableTypes == null || AvailableTypes.Count == 0)
                throw new InvalidOperationException("No Monster Types registered");

            CalcutateAdjustedEncounterChances();
            MonsterAvailability type = null;
            if (AvailableTypes.Count > 1)
            {
                float val = UnityEngine.Random.Range(0f, 1f);
                type = AvailableTypes.OrderBy(x => x.AdjustedEncounterChance)
                    .FirstOrDefault(x => val <= x.AdjustedEncounterChance);
            }
            else
                type = AvailableTypes[0];

            Image img = parent.GetComponent<Image>();
            MonsterSprite selectedSprite = null;
            if (img != null)
            {
                int index = UnityEngine.Random.Range(0, type.Sprites.Count);
                selectedSprite = type.Sprites[index];
                img.sprite = this.imageRepository.GetSprite(type.Sprites[index].Name);
            }
            MonsterBase existingComponent = parent.GetComponent<MonsterBase>();
            if (existingComponent != null)
                GameObject.Destroy(existingComponent);

            MonsterBase mb = CreateMonster(parent, type.Type);
            if (mb != null)
            {
                mb.MonsterType = type.Type;
                mb.MonsterName = selectedSprite.DisplayName;

                this.ApplyScaling(mb, powerScale);

                return mb;
            }
            else
                throw new Exception("Monster not created...");
        }

        public Sprite GetSpriteFromRepository(MonsterSprite spriteData)
        {
            return this.imageRepository.GetSprite(spriteData.Name);
        }

        /// <summary>
        /// For creating Bosses
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="monsterCreationData"></param>
        /// <returns></returns>
        internal MonsterBase Create(GameObject parent, MonsterCreationData monsterCreationData)
        {
            if (monsterCreationData == null)
                throw new ArgumentException("MonsterCreationData");

            Image img = parent.GetComponent<Image>();
            if (img != null)
                img.sprite = this.imageRepository.GetSprite(monsterCreationData.Sprite.Name);

            MonsterBase existingComponent = parent.GetComponent<MonsterBase>();
            if (existingComponent != null)
                GameObject.Destroy(existingComponent);

            MonsterBase mb = CreateMonster(parent, monsterCreationData.Sprite.DisplayName);
            if (mb == null)
                mb = CreateMonster(parent, monsterCreationData.Type);

            if (mb != null)
            {
                mb.MonsterType = monsterCreationData.Type;
                if (!string.IsNullOrEmpty(monsterCreationData.Sprite.DisplayName))
                    mb.MonsterName = monsterCreationData.Sprite.DisplayName;
                this.ApplyScaling(mb, monsterCreationData.PowerScale);

                if (monsterCreationData.XP.HasValue)
                    mb.XP = monsterCreationData.XP.Value;
                if (monsterCreationData.Gold.HasValue)
                    mb.Gold = monsterCreationData.Gold.Value;

                // all bosses capable of casting magic should have heal (except for glass cannon).
                if (mb.MonsterType == MonsterType.Balanced || mb.MonsterType == MonsterType.Mage || mb.MonsterType == MonsterType.GlassCannon)
                    mb.AddSpell("Heal");
                return mb;
            }
            else
                throw new Exception("Monster not created...");
        }

        private void ApplyScaling(MonsterBase mb, double scale)
        {
            MonsterStatCalculator calc = new MonsterStatCalculator();
            calc.PowerScale = scale;
            calc.Monster = mb;
            calc.MonType = mb.MonsterType;
            calc.Calculate();
            mb.CalculateDerivedStats();
            mb.DetermineSpells();
            int maxStat = Enumerable.Max(new int[] { mb.Stats.Str, mb.Stats.Vit, mb.Stats.Int });
            mb.Gold = (int)(maxStat * UnityEngine.Random.Range(1f, (2f + (float)scale)));
            mb.XP = (int)(maxStat * UnityEngine.Random.Range(1f, 2f + (float)scale));
        }

        private void CalcutateAdjustedEncounterChances()
        {
            var sorted = AvailableTypes.OrderBy(x => x.EncounterChance).ToList();
            for (int i = 0; i < sorted.Count(); i++)
            {
                if (i == 0)
                    sorted[i].AdjustedEncounterChance = sorted[i].EncounterChance;
                else
                    sorted[i].AdjustedEncounterChance = sorted[i].EncounterChance + sorted[i - 1].AdjustedEncounterChance;
            }
        }

        // wires up stat base and AI behavior
        private MonsterBase CreateMonster(GameObject parent, MonsterType t)
        {
            switch (t)
            {
                case MonsterType.Fighter:
                    return parent.AddComponent<FighterMonster>();
                case MonsterType.Mage:
                    return parent.AddComponent<MageMonster>();
                case MonsterType.Balanced:
                    return parent.AddComponent<BalancedMonster>();
                case MonsterType.GlassCannon:
                    return parent.AddComponent<GlassCannonMonster>();
                case MonsterType.SpeedDemon:
                    return parent.AddComponent<SpeedDemonMonster>();
                case MonsterType.Tank:
                    return parent.AddComponent<TankMonster>();
            }
            return null;
        }

        private MonsterBase CreateMonster(GameObject parent, string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
                return null;

            switch (displayName)
            {
                case "Abomination":
                    return parent.AddComponent<AbominationBehavior>();
                case "Chimera":
                    return parent.AddComponent<ChimeraBehavior>();
                case "Dark Beast":
                    return parent.AddComponent<DarkBeastBehavior>();
                case "Summoned Being":
                    return parent.AddComponent<SummonedBeingBehavior>();
                case "Arch-Fiend":
                    return parent.AddComponent<ArchFiendBehavior>();
            }

            return null;
        }
    }
}
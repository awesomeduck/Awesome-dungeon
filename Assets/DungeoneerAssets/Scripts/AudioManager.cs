using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class AudioManager : MonoBehaviour
    {
        private static SFX[] restrictOverlap = new SFX[] { SFX.WallBump };
        public AudioSource BGMSource;
        public List<AudioSource> SFXSources;
        public SettingsManager settings;

        [Header("BGM")]
        public AudioClip Town;
        public AudioClip Fight;
        public AudioClip Boss;
        public AudioClip Dungeon;
        public AudioClip Ending;

        [Header("Player Attack")]
        public AudioClip PlayerAttack;
        public AudioClip PlayerMiss;                

        [Header("Monster Attack")]
        public AudioClip MonsterAttack;
        public AudioClip MonsterMiss;        

        [Header("Spells")]
        public AudioClip healSpell;
        public AudioClip fireSpell;
        public AudioClip shieldSpell;
        public AudioClip scanSpell;
        public AudioClip exitSpell;

        [Header("Player Skills")]
        public AudioClip focusClip;
        public AudioClip meditateClip;
        public AudioClip FierceAttackClip;
        public AudioClip StunClip;
        public AudioClip DoubleAttackClip;
        public AudioClip ExtendedGuardClip;
        public AudioClip PerfectGuardClip;

        [Header("Items")]
        public AudioClip potion;
        public AudioClip Equip; 
        public AudioClip smokeBomb;
        public AudioClip cheapPurchase;
        public AudioClip expensivePurchase;
                          
        [Header("Navigation")]
        public AudioClip WallBump;
        public AudioClip footStep;
        public AudioClip ChestOpen;                                        
        public AudioClip stairsUp;
        public AudioClip stairsDown;

        [Header("UI")]
        public AudioClip buttonClick;
        public AudioClip buttonNegativeClick;

        [Header("Battle Finish")]
        public AudioClip normalFightWon;
        public AudioClip bossFightWon;
        public AudioClip runAway;
        public AudioClip gameOver;

        private void Start()
        {
            if (this.settings != null && this.settings.CurrentSettings != null)
            {
                this.CalibrateSourcesFromSettings();
            }
            else if (this.settings != null)
            {
                this.settings.Loaded += settings_Loaded;
            }
        }

        void settings_Loaded(object sender, EventArgs e)
        {
            this.CalibrateSourcesFromSettings();
            this.settings.Loaded -= settings_Loaded;
        }

        private void CalibrateSourcesFromSettings()
        {
            this.BGMSource.volume = this.settings.CurrentSettings.BGMVolume;
            this.BGMSource.spatialBlend = 0;
            foreach (AudioSource source in this.SFXSources)
            {
                source.spatialBlend = 0;
                source.volume = this.settings.CurrentSettings.SFXVolume;
            }
        }

        public void PlayBGM(BGM bgm)
        {
            switch (bgm)
            {
                case BGM.Town:
                    this.BGMSource.clip = this.Town;
                    this.BGMSource.Play();
                    break;
                case BGM.Fight:
                    this.BGMSource.clip = this.Fight;
                    this.BGMSource.Play();
                    break;
                case BGM.Boss:
                    this.BGMSource.clip = this.Boss;
                    this.BGMSource.Play();
                    break;
                case BGM.Dungeon:
                    this.BGMSource.clip = this.Dungeon;
                    this.BGMSource.Play();
                    break;
                case BGM.Ending:
                    this.BGMSource.clip = this.Ending;
                    this.BGMSource.Play();
                    break;
                default:
                    break;
            }
        }

        public void PlaySFX(SFX sfx)
        {
            AudioClip targetClip = this.GetClip(sfx);
            if (restrictOverlap.Contains(sfx))
            {
                if (this.SFXSources.Any(x => x.isPlaying && x.clip == targetClip))
                    return;
            }

            AudioSource source = this.SFXSources.FirstOrDefault(x => !x.isPlaying);
            if (source != null)
            {
                source.clip = targetClip;
                source.Play();
            }
        }

        // so it can be exposed to UI the event manager. 
        public void PlayButtonClick()
        {
            this.PlaySFX(SFX.ButtonClick);
        }

        private AudioClip GetClip(SFX sfx)
        {
            switch (sfx)
            {
                case SFX.PlayerAttack:
                    return this.PlayerAttack;
                case SFX.MonsterAttack:
                    return this.MonsterAttack;
                case SFX.ChestOpen:
                    return this.ChestOpen;
                case SFX.Equip:
                    return this.Equip;
                case SFX.WallBump:
                    return this.WallBump;
                case SFX.Potion:
                    return this.potion;
                case SFX.HealSpell:
                    return this.healSpell;
                case SFX.FireSpell:
                    return this.fireSpell;
                case SFX.ShieldSpell:
                    return this.shieldSpell;
                case SFX.ScanSpell:
                    return this.scanSpell;
                case SFX.GameOver:
                    return this.gameOver;
                case SFX.CheapPurchase:
                    return this.cheapPurchase;
                case SFX.ExpensivePurchase:
                    return this.expensivePurchase;
                case SFX.NormalFightWon:
                    return this.normalFightWon;
                case SFX.BossFightWon:
                    return this.bossFightWon;
                case SFX.Footstep:
                    return this.footStep;
                case SFX.RunAway:
                    return this.runAway;
                case SFX.SmokeBomb:
                    return this.smokeBomb;
                case SFX.StairsDown:
                    return this.stairsDown;
                case SFX.StairsUp:
                    return this.stairsUp;
                case SFX.ButtonClick:
                    return this.buttonClick;
                case SFX.ButtonNegativeClick:
                    return this.buttonNegativeClick;
                case SFX.ExitSpell:
                    return this.exitSpell;
                case SFX.PlayerMiss:
                    return this.PlayerMiss;
                case SFX.MonsterMiss:
                    return this.MonsterMiss;
                case SFX.Focus:
                    return this.focusClip;
                case SFX.Meditate:
                    return this.meditateClip;
                case SFX.FierceAttack:
                    return this.FierceAttackClip;
                case SFX.Stun:
                    return this.StunClip;
                case SFX.DoubleAttack:
                    return this.DoubleAttackClip;
                case SFX.ExtendedGuard:
                    return this.ExtendedGuardClip;
                case SFX.PerfectGuard:
                    return this.PerfectGuardClip;
                default:
                    break;

            }

            throw new InvalidOperationException("GetClip: " + sfx.ToString() + " not accounted for.");
        }

        public void StopBGM()
        {
            this.BGMSource.Stop();
        }

        public void StopSFX()
        {
            this.SFXSources.ForEach(x => x.Stop());
        }

        public void StopAll()
        {
            this.StopBGM();
            this.StopSFX();
        }
    }

    public enum BGM
    {
        Town,
        Fight,
        Boss,
        Dungeon,
        Ending
    }

    public enum SFX
    {
        PlayerAttack,
        MonsterAttack,
        ChestOpen,
        Equip,
        WallBump,
        Potion,
        HealSpell,
        FireSpell,
        ShieldSpell,
        ScanSpell,
        GameOver,
        CheapPurchase,
        ExpensivePurchase,
        NormalFightWon,
        BossFightWon,
        Footstep,
        RunAway,
        SmokeBomb,
        StairsUp,
        StairsDown,
        ButtonClick,
        ButtonNegativeClick,
        ExitSpell,
        PlayerMiss,
        MonsterMiss,
        Focus,
        Meditate,
        FierceAttack,
        Stun,
        DoubleAttack,
        ExtendedGuard,
        PerfectGuard
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public class SettingsScreen : MonoBehaviour
    {
        public SettingsManager settingsManager;
        public AudioManager audioManager;
        public PlayerNavigation playerNav;
        public Slider sfxSlider;
        public Slider bgmSlider;
        public Toggle chkIntersectionPause;

        private void OnEnable()
        {
            sfxSlider.value = this.settingsManager.CurrentSettings.SFXVolume;
            bgmSlider.value = this.settingsManager.CurrentSettings.BGMVolume;
            chkIntersectionPause.isOn = this.settingsManager.CurrentSettings.PauseAtIntersections;

            this.settingsManager.CurrentSettings.PropertyChanged += CurrentSettings_PropertyChanged;
            this.playerNav.pauseAtIntersectionsEnabled = this.settingsManager.CurrentSettings.PauseAtIntersections;
            this.audioManager.BGMSource.volume = this.settingsManager.CurrentSettings.BGMVolume;
            foreach (AudioSource sfxSource in this.audioManager.SFXSources)
                sfxSource.volume = this.settingsManager.CurrentSettings.SFXVolume;
        }

        private void OnDisable()
        {
            this.settingsManager.CurrentSettings.PropertyChanged -= CurrentSettings_PropertyChanged;
        }

        private void CurrentSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "BGMVolume":
                    this.audioManager.BGMSource.volume = this.settingsManager.CurrentSettings.BGMVolume;
                    break;
                case "SFXVolume":
                    foreach (AudioSource sfxSource in this.audioManager.SFXSources)
                        sfxSource.volume = this.settingsManager.CurrentSettings.SFXVolume;
                    break;
                case "PauseAtIntersections":
                    this.playerNav.pauseAtIntersectionsEnabled = this.settingsManager.CurrentSettings.PauseAtIntersections;
                    break;
            }
        }

        private void Update()
        {
            if (this.settingsManager.CurrentSettings.SFXVolume != sfxSlider.value)
                this.settingsManager.CurrentSettings.SFXVolume = sfxSlider.value;
            if (this.settingsManager.CurrentSettings.BGMVolume != bgmSlider.value)
                this.settingsManager.CurrentSettings.BGMVolume = bgmSlider.value;
            if (this.settingsManager.CurrentSettings.PauseAtIntersections != this.chkIntersectionPause.isOn)
                this.settingsManager.CurrentSettings.PauseAtIntersections = this.chkIntersectionPause.isOn;

        }

        public void RestoreDefaults()
        {
            this.settingsManager.CurrentSettings.SFXVolume = 1.0f;
            this.settingsManager.CurrentSettings.BGMVolume = 0.6f;
            this.settingsManager.CurrentSettings.PauseAtIntersections = false;
            this.sfxSlider.value = this.settingsManager.CurrentSettings.SFXVolume;
            this.bgmSlider.value = this.settingsManager.CurrentSettings.BGMVolume;
            this.chkIntersectionPause.isOn = this.settingsManager.CurrentSettings.PauseAtIntersections;
        }

        public void SaveChanges()
        {
            this.settingsManager.Save();
        }
    }
}
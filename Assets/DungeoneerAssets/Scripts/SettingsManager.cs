using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace DungeoneerAssets
{
    public class SettingsManager : MonoBehaviour
    {
        private string filePath;
        public Settings CurrentSettings { get; set; }
        public event EventHandler Loaded;

        private void Start()
        {
            this.filePath = Path.Combine(Application.persistentDataPath, "settings.txt");
            if (File.Exists(filePath))
            {
                try
                {
                    this.LoadSavedSettings();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            if (this.CurrentSettings == null)
            {
                this.CreateDefaultSettings();
                this.Save();
            }

            this.OnLoaded();
        }

        protected void OnLoaded()
        {
            EventHandler temp = Loaded;
            if (temp != null)
                temp(this, null);
        }

        private void CreateDefaultSettings()
        {
            this.CurrentSettings = new Settings();
            this.CurrentSettings.Orientation = ControlOrientation.RightHanded;
            this.CurrentSettings.BGMVolume = 0.6f;
            this.CurrentSettings.SFXVolume = 1.0f;
            this.CurrentSettings.PauseAtIntersections = false;
        }

        public void Save()
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Settings));
                xs.Serialize(fs, this.CurrentSettings);
            }
        }

        private Settings Load()
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Settings));
                return (Settings)xs.Deserialize(fs);
            }
        }

        public void LoadSavedSettings()
        {
            Settings settings = null;
            try
            {
                settings = this.Load();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            if (settings != null)
            {
                this.CopySettingsValues(settings);
            }
        }

        /// <summary>
        /// do the copy like this so that all property changed notifications get fired. 
        /// </summary>
        /// <param name="settings"></param>
        private void CopySettingsValues(Settings settings)
        {
            if (settings == null)
                throw new ArgumentException("Settings");

            if (this.CurrentSettings == null)
                this.CurrentSettings = new Settings();

            this.CurrentSettings.Orientation = settings.Orientation;
            this.CurrentSettings.BGMVolume = settings.BGMVolume;
            this.CurrentSettings.SFXVolume = settings.SFXVolume;
            this.CurrentSettings.PauseAtIntersections = settings.PauseAtIntersections;
        }
    }

    [Serializable]
    public class Settings : INotifyPropertyChanged
    {
        private ControlOrientation orientation;
        private float bgmVolume;
        private float sfxVolume;
        private bool pauseAtIntersections;
        public event PropertyChangedEventHandler PropertyChanged;

        public ControlOrientation Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                this.orientation = value;
                OnPropertyChanged("Orientation");
            }
        }

        public float BGMVolume
        {
            get { return this.bgmVolume; }
            set { if (this.bgmVolume != value) { this.bgmVolume = value; OnPropertyChanged("BGMVolume"); } }
        }

        public float SFXVolume
        {
            get { return this.sfxVolume; }
            set { if (this.sfxVolume != value) { this.sfxVolume = value; OnPropertyChanged("SFXVolume"); } }
        }

        public bool PauseAtIntersections
        {
            get { return this.pauseAtIntersections; }
            set { if (this.pauseAtIntersections != value) { this.pauseAtIntersections = value; OnPropertyChanged("PauseAtIntersections"); } }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum ControlOrientation
    {
        LeftHanded,
        RightHanded
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace DungeoneerAssets
{
    public class SaveGameManager
    {
        private string filePathBase = Path.Combine(Application.persistentDataPath, "save{0}.txt");

        public SaveGameState Load(int index)
        {
            string targetFile = string.Format(this.filePathBase, index);
            return this.LoadFile(targetFile);
        }

        public void Save(int index, SaveGameState state)
        {
            string targetFile = string.Format(this.filePathBase, index);
            state.Index = index;
            using (FileStream fs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                XmlSerializer xs = new XmlSerializer(typeof(SaveGameState));
                xs.Serialize(fs, state);
            }
        }

        private SaveGameState LoadFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer xs = new XmlSerializer(typeof(SaveGameState));
                return xs.Deserialize(fs) as SaveGameState;
            }
        }

        public List<SaveGameState> GetAllSaves()
        {
            List<SaveGameState> result = new List<SaveGameState>();
            string[] files = Directory.GetFiles(Application.persistentDataPath, "save*", SearchOption.TopDirectoryOnly);
            if (files != null && files.Length > 0)
                foreach (string file in files)
                    result.Add(this.LoadFile(file));

            return result;
        }

        public bool HasSaveFiles()
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath, "save*", SearchOption.TopDirectoryOnly);
            return files != null && files.Length > 0;
        }
    }
}
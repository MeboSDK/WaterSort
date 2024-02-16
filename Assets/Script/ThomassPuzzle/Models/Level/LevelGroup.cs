using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using ThomassPuzzle.Models;

namespace ThomassPuzzle.Models.Level
{
    [Serializable]
    public struct LevelGroup : IEnumerable<Level>
    {
        public List<Level> levels;

        [DoNotSerialize] public static LevelGroup LevelData { get { return GetDataFromJson(); } }

        public IEnumerator<Level> GetEnumerator()
        {
            return levels?.GetEnumerator() ?? Enumerable.Empty<Level>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static LevelGroup GetDataFromJson()
        {
            string filePath = string.Empty;
#if UNITY_ANDROID

           // filePath = Path.Combine(Application.streamingAssetsPath, "Resources/levels.json");
             filePath = "file://" + Application.persistentDataPath + "/" + "Resources/levels.json";

            if (System.IO.File.Exists(filePath))
            {
                // The file exists -> run event
                Debug.Log("Arsebobs");

            }
            else
            {
                Debug.Log("Ar Arsebobs");
                // The file does not exist -> run event
            } //event     
#endif
#if UNITY_EDITOR
            filePath = Path.Combine(Application.dataPath, "Resources/levels.json");

#endif
            Debug.Log(filePath);
            string jsonString = File.ReadAllText(filePath);
            jsonString = jsonString.Replace("\\", "");

            return JsonUtility.FromJson<LevelGroup>(jsonString);
        }
    }
}

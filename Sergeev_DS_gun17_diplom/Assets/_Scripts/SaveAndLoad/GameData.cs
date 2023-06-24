using System;
using System.Collections.Generic;
using UnityEngine.Serialization;


namespace Metroidvania.GameData
{
    [Serializable]
    public class GameData
    {
        public int playerHealth;
        public SerializableDictionary<string, int> inventory;
        public float xPlayerPosition;
        public float yPlayerPosition;
        public SerializableDictionary<string, bool> chests;
        public SerializableDictionary<string, bool> campfires;
        public string lastScene;
        public SerializableDictionary<string, float> audioVolume;

        public GameData()
        {
            playerHealth = 0;
            inventory = new SerializableDictionary<string, int>();
            xPlayerPosition = 0;
            yPlayerPosition = 0;
            chests = new SerializableDictionary<string, bool>();
            campfires = new SerializableDictionary<string, bool>();
            audioVolume = new SerializableDictionary<string, float>();

        }
    }
}
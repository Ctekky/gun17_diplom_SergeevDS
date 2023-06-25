using System;
using System.Collections.Generic;
using UnityEngine.Serialization;


namespace Metroidvania.GameData
{
    [Serializable]
    public class GameData
    {
        public int playerHealth;
        public int jumpCount;
        public string lastScene;
        public float xPlayerPosition;
        public float yPlayerPosition;
        public SerializableDictionary<string, bool> chests;
        public SerializableDictionary<string, bool> campfires;
        public SerializableDictionary<string, bool> levers;
        public SerializableDictionary<string, bool> doors;
        public SerializableDictionary<string, int> inventory;
        public SerializableDictionary<string, float> audioVolume;
        
        public GameData()
        {
            playerHealth = 0;
            jumpCount = 1;
            lastScene = "";
            xPlayerPosition = 0;
            yPlayerPosition = 0;
            chests = new SerializableDictionary<string, bool>();
            campfires = new SerializableDictionary<string, bool>();
            levers = new SerializableDictionary<string, bool>();
            doors = new SerializableDictionary<string, bool>();
            audioVolume = new SerializableDictionary<string, float>();
            inventory = new SerializableDictionary<string, int>();
        }
    }
}
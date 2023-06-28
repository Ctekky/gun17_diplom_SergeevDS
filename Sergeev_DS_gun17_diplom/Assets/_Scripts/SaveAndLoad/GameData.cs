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
        public float playingTime;
        public SerializableDictionary<string, float> xPlayerPosition;
        public SerializableDictionary<string, float> yPlayerPosition;
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
            playingTime = 0;
            xPlayerPosition = new SerializableDictionary<string, float>();
            yPlayerPosition = new SerializableDictionary<string, float>();
            chests = new SerializableDictionary<string, bool>();
            campfires = new SerializableDictionary<string, bool>();
            levers = new SerializableDictionary<string, bool>();
            doors = new SerializableDictionary<string, bool>();
            audioVolume = new SerializableDictionary<string, float>();
            inventory = new SerializableDictionary<string, int>();
        }
    }
}
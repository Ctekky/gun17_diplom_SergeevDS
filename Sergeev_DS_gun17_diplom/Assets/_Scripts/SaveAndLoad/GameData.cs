using System;

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
        public SerializableDictionary<string, int> currentAmmo;
        public SerializableDictionary<string, int> potion1;
        public SerializableDictionary<string, int> potion2;
        public SerializableDictionary<string, int> potion3;
        public SerializableDictionary<string, int> potion4;

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
            currentAmmo = new SerializableDictionary<string, int>();
            potion1 = new SerializableDictionary<string, int>();
            potion2 = new SerializableDictionary<string, int>();
            potion3 = new SerializableDictionary<string, int>();
            potion4 = new SerializableDictionary<string, int>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.GameData;
using Metroidvania.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Metroidvania.Managers
{
    public class SaveManager : MonoBehaviour
    {
        private GameData.GameData _gameData;
        private FileDataHandler _dataHandler;
        private string _fileName; 
        private List<ISaveAndLoad> _saveInterfacesInScripts;
        [SerializeField] private bool encryptData;
        public string sceneName;

        private void Awake()
        {
            _fileName = "data.test";
            encryptData = false;
        }

        private void Start()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, encryptData);
            _saveInterfacesInScripts = FindAllSaveAndLoadInterfaces();
            LoadGame();
        }

        public void AddScriptToList(ISaveAndLoad script)
        {
            _saveInterfacesInScripts.Add(script);
        }

        private void NewGame()
        {
            _gameData = new GameData.GameData
            {
                playerHealth = 125
            };
        }

        public bool CheckForSavedData()
        {
            _gameData = _dataHandler.Load();
            return _gameData != null;
        }

        public void LoadGame()
        {
            _gameData = _dataHandler.Load();
            if (_gameData == null)
            {
                Debug.Log("No save data found!");
                NewGame();
            }
            foreach (var loadScript in _saveInterfacesInScripts)
            {
                loadScript.LoadData(_gameData);
            }
        }

        public void SaveGame()
        { 
            foreach (var saveScript in _saveInterfacesInScripts)
            {
                saveScript.SaveData(ref _gameData);
            }
            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<ISaveAndLoad> FindAllSaveAndLoadInterfaces()
        {
            var saveInterfacesInScripts = FindObjectsOfType<MonoBehaviour>().OfType<ISaveAndLoad>();
            return new List<ISaveAndLoad>(saveInterfacesInScripts);
        }

        [ContextMenu("Delete Save Data")]
        public void DeleteSavedData()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, encryptData);
            _dataHandler.Delete();
        }
    }
}
using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace Metroidvania.GameData
{
    public class FileDataHandler
    {
        private string _dataDirPath = "";
        private string _dataFileName = "";
        private bool _encryptData = false;
        private string _codeWord = "bobrcurva";

        public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _encryptData = encryptData;
        }

        private string EncryptDecrypt(string data)
        {
            var modifiedData = "";
            for (var i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ _codeWord[i % _codeWord.Length]);
            }

            return modifiedData; 
        }

        public void Save(GameData gameData)
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                var dataToStore = JsonUtility.ToJson(gameData, true);
                if (_encryptData) dataToStore = EncryptDecrypt(dataToStore);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.Log($"Error on trying to save data to file {fullPath} \n {e}");
            }
            
        }

        public GameData Load()
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            GameData loadData = null;
            if (!File.Exists(fullPath))
            {
                Debug.Log($"There is no save file {fullPath}");
                return null;
            }
            try
            {
                var dataToLoad = "";
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (_encryptData) dataToLoad = EncryptDecrypt(dataToLoad);
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
                
            }
            catch (Exception e)
            {
                Debug.Log($"Error on trying to load data from file {fullPath} \n {e}");
            }
            return loadData;
        }

        public void Delete()
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            if(File.Exists(fullPath)) File.Delete(fullPath);
        }
        
    }
    
}


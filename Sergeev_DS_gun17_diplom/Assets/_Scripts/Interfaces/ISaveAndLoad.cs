using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Interfaces
{
    public interface ISaveAndLoad
    {
        void LoadData(GameData.GameData gameData);
        void SaveData(ref GameData.GameData gameData);
    }
    
}

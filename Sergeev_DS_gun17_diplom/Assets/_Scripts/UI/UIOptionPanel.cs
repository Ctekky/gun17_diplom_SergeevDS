using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Metroidvania.UI
{
    public class UIOptionPanel : MonoBehaviour
    {
        public event Action GameSavedFromUI;
        public event Action GameLoadedFromUI;
        public event Action GameEndedFromUI;

        public void OnGameSaved()
        {
            GameSavedFromUI?.Invoke();
        }

        public void OnGameLoaded()
        {
            GameLoadedFromUI?.Invoke();
        }

        public void OnGameEnded()
        {
            GameSavedFromUI?.Invoke();
            GameEndedFromUI?.Invoke();
        }
    }
    
}

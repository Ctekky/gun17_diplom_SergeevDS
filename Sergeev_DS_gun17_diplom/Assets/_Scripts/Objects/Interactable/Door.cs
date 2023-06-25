using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.Interfaces;
using UnityEngine;

namespace Metroidvania.Common.Objects
{
    public class Door : MonoBehaviour, ISaveAndLoad
    {
        [SerializeField] private Animator animator;
        [SerializeField] private bool isOpen;
        private static readonly int IsOpen = Animator.StringToHash("isOpen");

        public void ChangeState(bool state)
        {
            isOpen = state;
            animator.SetBool(IsOpen, isOpen);
        }
        public void LoadData(GameData.GameData gameData)
        {
            foreach (var pair in gameData.doors.Where(pair => pair.Key == gameObject.name))
            {
                isOpen = pair.Value;
                if(isOpen) animator.SetBool(IsOpen, true);
            }
        }
        public void SaveData(ref GameData.GameData gameData)
        {
            if (gameData.doors.TryGetValue(gameObject.name, out var value))
            {
                gameData.doors[gameObject.name] = isOpen;
            }
            else
            {
                gameData.doors.Add($"{gameObject.name}", isOpen);    
            }
        }
    }
    
}

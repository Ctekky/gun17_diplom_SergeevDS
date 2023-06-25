using System;
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
        [SerializeField] private bool isIdle;
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        private static readonly int IsIdle = Animator.StringToHash("isIdle");

        private void Start()
        {
            animator.SetBool(IsOpen, isOpen);
            animator.SetBool(IsIdle, isIdle);
        }

        public void ChangeState()
        {
            isOpen = !isOpen;
            isIdle = false;
            animator.SetBool(IsOpen, isOpen);
            animator.SetBool(IsIdle, isIdle);
        }

        private void SetState(bool state)
        {
            isOpen = state;
            isIdle = false;
            animator.SetBool(IsOpen, isOpen);
            animator.SetBool(IsIdle, isIdle);
        }

        public void OnAnimationEnd()
        {
            isIdle = true;
            animator.SetBool(IsIdle, isIdle);
        }
        private void OnValidate()
        {
            name = transform.parent.name;
        }
        public void LoadData(GameData.GameData gameData)
        {
            foreach (var pair in gameData.doors.Where(pair => pair.Key == gameObject.name))
            {
                SetState(pair.Value);
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SetState(isOpen);
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
            var currentScene = SceneManager.GetActiveScene().name;
            var dictKey = currentScene + "_" + gameObject.name;
            foreach (var pair in gameData.doors.Where(pair => pair.Key == dictKey))
            {
                SetState(pair.Value);
            }
        }

        public void SaveData(ref GameData.GameData gameData)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var dictKey = currentScene + "_" + gameObject.name;
            if (gameData.doors.TryGetValue(dictKey, out var value))
            {
                gameData.doors[dictKey] = isOpen;
            }
            else
            {
                gameData.doors.Add($"{dictKey}", isOpen);
            }
        }
    }
}
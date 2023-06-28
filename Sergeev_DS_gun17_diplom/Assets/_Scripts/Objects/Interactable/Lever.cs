using System;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Metroidvania.Common.Objects
{
    public class Lever : MonoBehaviour, IInteractable, ISaveAndLoad
    {
        [SerializeField] private List<Door> doors;
        [SerializeField] private bool isOpen = false;
        [SerializeField] private Animator animator;
        public event Action<LootType, Vector2> Opened;
        public event Action<Vector2> Used;
        public event Action<Transform> Saved;
        public event Action LeverActivated;
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        private static readonly int IsOpening = Animator.StringToHash("isOpening");
        private static readonly int IsClosing = Animator.StringToHash("isClosing");
        private static readonly int IsClose = Animator.StringToHash("isClose");

        public void Interact()
        {
            LeverActivated?.Invoke();
            if (isOpen)
            {
                animator.SetBool(IsOpen, false);
                animator.SetBool(IsClosing, true);
                isOpen = false;
            }
            else
            {
                animator.SetBool(IsClose, false);
                animator.SetBool(IsOpening, true);
                isOpen = true;
            }
        }

        private void OnValidate()
        {
            name = transform.parent.name;
        }

        public bool ReturnState()
        {
            return isOpen;
        }

        private void ChangeDoorState()
        {
            foreach (var door in doors)
            {
                door.ChangeState();
            }
        }

        public void OnAnimationEnd()
        {
            animator.SetBool(isOpen ? IsOpening : IsClosing, false);
            animator.SetBool(isOpen ? IsOpen : IsClose, true);
            ChangeDoorState();
        }

        public void LoadData(GameData.GameData gameData)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var dictKey = currentScene + "_" + gameObject.name;
            foreach (var pair in gameData.levers.Where(pair => pair.Key == dictKey))
            {
                isOpen = pair.Value;
                animator.SetBool(isOpen ? IsOpen : IsClose, true);
            }
        }

        public void SaveData(ref GameData.GameData gameData)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var dictKey = currentScene + "_" + gameObject.name;
            if (gameData.levers.TryGetValue(dictKey, out var value))
            {
                gameData.levers[dictKey] = isOpen;
            }
            else
            {
                gameData.levers.Add($"{dictKey}", isOpen);
            }
        }
    }
}
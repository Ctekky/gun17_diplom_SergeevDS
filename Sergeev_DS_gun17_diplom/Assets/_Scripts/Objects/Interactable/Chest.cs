using System;
using System.Linq;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Metroidvania.Common.Objects
{
    public class Chest : MonoBehaviour, IInteractable, ISaveAndLoad
    {
        [SerializeField] private bool _state;
        [SerializeField] private Animator animator;
        [SerializeField] private LootType lootType;
        private static readonly int IsOpening = Animator.StringToHash("isOpening");
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        private PopUpText _popUpText;
        [Inject] private AudioManager _audioManager;
        public event Action<LootType, Vector2> Opened;
        public event Action<Vector2> Used;
        public event Action<Transform> Saved;

        private void Awake()
        {
            _state = false;
        }

        private void OnValidate()
        {
            name = transform.parent.name;
        }

        private void Start()
        {
            if (_state) animator.SetBool(IsOpen, true);
            ChangePopUpTextState();
        }

        private void ChangePopUpTextState()
        {
            _popUpText = GetComponent<PopUpText>();
            _popUpText.GetSetHideText = _state;
        }

        public void Interact()
        {
            if (_state) return;
            _audioManager.PlaySFX((int)SFXSlots.OpenChest);
            animator.SetBool(IsOpening, true);
            _state = true;
        }

        public bool ReturnState()
        {
            return _state;
        }

        public void OnAnimationEnd()
        {
            animator.SetBool(IsOpening, false);
            animator.SetBool(IsOpen, true);
            Opened?.Invoke(lootType, transform.position);
        }

        public void LoadData(GameData.GameData gameData)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var dictKey = currentScene + "_" + gameObject.name;
            foreach (var pair in gameData.chests.Where(pair => pair.Key == dictKey))
            {
                _state = pair.Value;
                if (_state) animator.SetBool(IsOpen, true);
                ChangePopUpTextState();
            }
        }

        public void SaveData(ref GameData.GameData gameData)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var dictKey = currentScene + "_" + gameObject.name;
            if (gameData.chests.TryGetValue(dictKey, out var value))
            {
                gameData.chests[dictKey] = _state;
            }
            else
            {
                gameData.chests.Add($"{dictKey}", _state);
            }
        }
    }
}
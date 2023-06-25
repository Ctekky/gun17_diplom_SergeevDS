using System;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using UnityEngine;
using Zenject;

namespace Metroidvania.Common.Objects
{
    public class DoubleJump : MonoBehaviour,IInteractable,ISaveAndLoad
    {
        public event Action<LootType, Vector2> Opened;
        public event Action<Vector2> Used;
        public event Action<Transform> Saved;
        [Inject] private Player.Player _player;
        [SerializeField] private bool _state;
        [SerializeField] private Animator animator;
        [SerializeField] private LootType lootType;
        private static readonly int IsOpening = Animator.StringToHash("isOpening");
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        private PopUpText _popUpText;
        [Inject] private AudioManager _audioManager;
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
            if(_state) animator.SetBool(IsOpen, true);
            ChangePopUpTextState();
        }

        private void ChangePopUpTextState()
        {
            _popUpText = GetComponent<PopUpText>();
            _popUpText.GetSetHideText = _state;
        }

        public void Interact()
        {
            if(_state) return;
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
            _player.AddDoubleJump();
        }
        public void LoadData(GameData.GameData gameData)
        {
            foreach (var pair in gameData.chests)
            {
                if (pair.Key == gameObject.name)
                {
                    _state = pair.Value;
                    if(_state) animator.SetBool(IsOpen, true);
                    ChangePopUpTextState();
                }
            }
        }
        public void SaveData(ref GameData.GameData gameData)
        {
            if (gameData.chests.TryGetValue(gameObject.name, out var value))
            {
                gameData.chests[gameObject.name] = _state;
            }
            else
            {
                gameData.chests.Add($"{gameObject.name}", _state);    
            }
        }
    }
    
}
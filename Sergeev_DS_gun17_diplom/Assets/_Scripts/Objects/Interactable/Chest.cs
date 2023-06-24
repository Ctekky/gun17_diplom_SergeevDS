using System;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Metroidvania.Combat.Objects
{
    public class Chest : MonoBehaviour, IInteractable, ISaveAndLoad
    {
        [SerializeField] private bool _state;
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private LootType lootType;
        private static readonly int IsOpening = Animator.StringToHash("isOpening");
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        [Inject] private AudioManager _audioManager;
        public event Action<LootType, Vector2> Opened;
        public event Action<Vector2> Used;
        public event Action<Transform> Saved;

        private void Awake()
        {
            _state = false;
        }

        public void Interact()
        {
            if(_state) return;
            _audioManager.PlaySFX((int)SFXSlots.OpenChest);
            animator.SetBool(IsOpening, true);
            _state = true;
        }

        private void OnValidate()
        {
            name = transform.parent.name;
        }

        private void Start()
        {
            if(_state) animator.SetBool(IsOpen, true);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if(_state) return;
            text.text = "Press E to open chest";
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            text.text = "";
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
            foreach (var pair in gameData.chests)
            {
                if (pair.Key == gameObject.name)
                {
                    _state = pair.Value;
                    if(_state) animator.SetBool(IsOpen, true);
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
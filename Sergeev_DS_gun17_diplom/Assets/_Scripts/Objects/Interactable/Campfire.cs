using System;
using Metroidvania.BaseUnit;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Metroidvania.Combat.Objects
{
    public class Campfire : MonoBehaviour, IInteractable, ISaveAndLoad
    {
        [Inject] private Player.Player _player;
        private bool _state;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Animator animator;
        private static readonly int Active = Animator.StringToHash("Active");
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
            if(_state) animator.SetBool(Active, true);
        }
        public void Interact()
        {
            var position = transform.position;
            _player.SetLastSpawnPoint(position);
            _player.Unit.GetUnitComponent<UnitStats>().RestoreHealth();
            animator.SetBool(Active, true);
            Saved?.Invoke(transform);
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            text.text = "Press E to save game";
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            text.text = "";
        }
        public void SetState(bool state)
        {
            _state = state;
            animator.SetBool(Active, _state);
        }
        
        public bool ReturnState()
        {
            return true;
        }
        
        public void LoadData(GameData.GameData gameData)
        {
            foreach (var pair in gameData.campfires)
            {
                if (pair.Key == gameObject.name)
                {
                    _state = pair.Value;
                    animator.SetBool(Active, _state);
                }
            }
        }
        public void SaveData(ref GameData.GameData gameData)
        {
            if (gameData.campfires.TryGetValue(gameObject.name, out var value))
            {
                gameData.campfires[gameObject.name] = _state;
            }
            else
            {
                gameData.campfires.Add($"{gameObject.name}", _state);    
            }
        }

    }
    
}

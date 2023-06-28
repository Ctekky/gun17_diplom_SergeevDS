using System;
using System.Linq;
using Metroidvania.BaseUnit;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Metroidvania.Common.Objects
{
    public class Campfire : MonoBehaviour, IInteractable, ISaveAndLoad
    {
        [Inject] private Player.Player _player;
        private bool _state;
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
            if (_state) animator.SetBool(Active, true);
        }

        public void Interact()
        {
            var position = transform.position;
            _player.SetLastSpawnPoint(position);
            _player.Unit.GetUnitComponent<UnitStats>().RestoreHealth();
            animator.SetBool(Active, true);
            Saved?.Invoke(transform);
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
            var currentScene = SceneManager.GetActiveScene().name;
            var dictKey = currentScene + "_" + gameObject.name;
            foreach (var pair in gameData.campfires.Where(pair => pair.Key == dictKey))
            {
                _state = pair.Value;
                animator.SetBool(Active, _state);
            }
        }

        public void SaveData(ref GameData.GameData gameData)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var dictKey = currentScene + "_" + gameObject.name;
            if (gameData.campfires.TryGetValue(dictKey, out var value))
            {
                gameData.campfires[dictKey] = _state;
            }
            else
            {
                gameData.campfires.Add($"{dictKey}", _state);
            }
        }
    }
}
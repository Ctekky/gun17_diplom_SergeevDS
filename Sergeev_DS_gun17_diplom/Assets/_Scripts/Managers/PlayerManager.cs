using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Metroidvania.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [Inject] private Player.Player _player;
        public Player.Player player;
        private void Start()
        {
            player = _player;
        }
    }
}

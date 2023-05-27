using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cinemachine;

namespace Metroidvania.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera playerCamera;

        [Inject]
        private void Construct(Player.Player player)
        {
            playerCamera.Follow = player.transform;
        }
    }
}




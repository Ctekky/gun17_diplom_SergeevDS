using System;
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

        [Inject] private Player.Player _player;
        private void Start()
        {
            playerCamera.Follow = _player.transform;
        }
        
        private void OnEndAiming()
        {
            StartCoroutine(ChangeCamerOrtoSize(8, 2));
        }

        private void OnAiming()
        {
            StartCoroutine(ChangeCamerOrtoSize(15, 2));
        }
        private void OnEnable()
        {
            _player.Aiming += OnAiming;
            _player.EndAiming += OnEndAiming;
        }
        private void OnDisable()
        {
            _player.Aiming -= OnAiming;
            _player.EndAiming -= OnEndAiming;
        }

        private IEnumerator ChangeCamerOrtoSize(float result, float seconds)
        {
            var currentOrto = playerCamera.m_Lens.OrthographicSize;
            float timeElapsed = 0;
            while (timeElapsed < seconds)
            {
                playerCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentOrto, result, timeElapsed / seconds);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            playerCamera.m_Lens.OrthographicSize = result;
        }
    }
}




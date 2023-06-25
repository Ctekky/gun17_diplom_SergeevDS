using Metroidvania.Managers;
using UnityEngine;
using Zenject;

namespace Metroidvania.Audio
{
    public class AreaOfSound : MonoBehaviour
    {
        [Inject] private AudioManager _audioManager;
        [SerializeField] private SFXSlots sfxSlot;
        [SerializeField] private BGMSlots bgmSlot;
        [SerializeField] private bool playSFX;
        [SerializeField] private bool playBGM;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponentInParent<Player.Player>() != true) return;
            if (playSFX) _audioManager.PlaySFX((int)sfxSlot);
            if (playBGM) _audioManager.PlayBGM((int)bgmSlot);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponentInParent<Player.Player>() != true) return;
            if (playSFX) _audioManager.StopSFXWithTime((int)sfxSlot);
            if (playBGM) _audioManager.StopAllBGM();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

namespace Metroidvania.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<AudioSource> sfx;
        [SerializeField] private List<AudioSource> bgm;
        [SerializeField] private float minDistanceSfx;
        public bool playBGM;
        private int _bgmIndex;
        private Player.Player _player;
        private bool _canPlaySfx;

        public void SetPlayer(Player.Player player)
        {
            _player = player;
        }

        private void Start()
        {
            Invoke(nameof(AllowSfx), 1f);
        }

        private void Update()
        {
            if (!playBGM) StopAllBGM();
            else
            {
                if (!bgm[_bgmIndex].isPlaying)
                    PlayBGM(_bgmIndex);
            }
        }

        public void PlayRandomBGM()
        {
            var index = Random.Range(0, bgm.Count);
            PlayBGM(index);
        }

        private IEnumerator DecreaseVolume(AudioSource audioSource)
        {
            var defaultVolume = audioSource.volume;
            while (audioSource.volume > .1f)
            {
                var volume = audioSource.volume;
                volume -= volume * .2f;
                audioSource.volume = volume;
                yield return new WaitForSeconds(.25f);
                if (audioSource.volume >= .1f) continue;
                audioSource.Stop();
                audioSource.volume = defaultVolume;
                break;
            }
        }

        public void PlaySfx(int sfxIndex)
        {
            if (!_canPlaySfx) return;
            if (sfx[sfxIndex].isPlaying) return;
            PlayAudio(sfxIndex, sfx);
        }

        public void PlaySfx(int sfxIndex, Transform source)
        {
            if (!_canPlaySfx) return;
            if (sfx[sfxIndex].isPlaying) return;
            var check = Vector2.Distance(Vector3ToVector2(_player.transform), Vector3ToVector2(source));
            if (source != null && check > minDistanceSfx) return;
            PlayAudio(sfxIndex, sfx);
        }

        private Vector2 Vector3ToVector2(Transform target)
        {
            var position = target.position;
            return new Vector2(position.x, position.y);
        }

        public void StopSfx(int sfxIndex)
        {
            if (!sfx[sfxIndex].isPlaying) return;
            StopAudio(sfxIndex, sfx);
        }

        public void StopSfxWithTime(int sfxIndex)
        {
            if (!sfx[sfxIndex].isPlaying) return;
            StartCoroutine(DecreaseVolume(sfx[sfxIndex]));
        }

        public void PlayBGM(int bgmIndex)
        {
            _bgmIndex = bgmIndex;
            StopAllBGM();
            PlayAudio(bgmIndex, bgm);
        }

        public void StopAllBGM()
        {
            foreach (var audioSource in bgm)
            {
                audioSource.Stop();
            }
        }

        private void PlayAudio(int index, List<AudioSource> audioList)
        {
            if (AudioInList(index, audioList))
            {
                audioList[index].pitch = Random.Range(0.85f, 1.2f);
                audioList[index].Play();
            }
        }

        private void StopAudio(int index, List<AudioSource> audioList)
        {
            if (AudioInList(index, audioList)) audioList[index].Stop();
        }

        private bool AudioInList(int index, ICollection audioList)
        {
            return index < audioList.Count;
        }

        private void AllowSfx()
        {
            _canPlaySfx = true;
        }
    }
}
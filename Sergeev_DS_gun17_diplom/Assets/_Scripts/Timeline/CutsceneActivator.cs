using System;
using Metroidvania.Common.Objects;
using UnityEngine;
using UnityEngine.Playables;

namespace Metroidvania.Timeline
{
    public class CutsceneActivator : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;
        [SerializeField] private Lever lever;


        private void OnEnable()
        {
            lever.LeverActivated += PlayCutscene;
        }

        private void OnDisable()
        {
            lever.LeverActivated -= PlayCutscene;
        }

        private void PlayCutscene()
        {
            director.Play();
        }
    }
}
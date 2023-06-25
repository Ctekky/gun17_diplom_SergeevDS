using System;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Metroidvania.UI
{
    public class UIVolumeSlider : MonoBehaviour
    {
        public Slider slider;
        public string parameter;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private float multiplier;
        
        public void SliderValue(float value)
        {
            audioMixer.SetFloat(parameter, Mathf.Log10(value) * multiplier);
        }
    }
    
}

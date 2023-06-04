using System;
using UnityEngine;
using UnityEngine.UI;

namespace Metroidvania.BaseUnit
{
    public class UI : UnitComponent
    {
        private Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        private Movement _movement;
        private UnitStats UnitStats => _unitStats ? _unitStats : Unit.GetUnitComponent<UnitStats>(ref _unitStats);
        private UnitStats _unitStats;
        private RectTransform _rectTransform;
        private Slider _slider;

        protected override void Start()
        {
            base.Start();
            _rectTransform = GetComponent<RectTransform>();
            _slider = GetComponentInChildren<Slider>();
            UpdateHealthUI();
        }
        private void UpdateHealthUI()
        {
            _slider.maxValue = UnitStats.GetMaxHealthValue();
            _slider.value = UnitStats.GetCurrentHealth();
        }
        private void OnEnable()
        {
            Movement.onFlipped += FlipUI;
            UnitStats.OnDecreaseHealth += UpdateHealthUI;
        }
        private void OnDisable()
        {
            Movement.onFlipped -= FlipUI;
            UnitStats.OnDecreaseHealth -= UpdateHealthUI;
        }
        private void FlipUI()
        {
            _rectTransform.Rotate(0f, 180,0f);
        }
    }
}
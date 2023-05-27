using System;
using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class Stats : UnitComponent
    {
        public event Action OnHealthZero;
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;

        protected override void Awake()
        {
            base.Awake();
            currentHealth = maxHealth;
        }
        public void DecreaseHealth(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnHealthZero?.Invoke();
                Debug.Log("Health is 0!");
            }
        }
        public void IncreaseHealth(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        }
    }
}



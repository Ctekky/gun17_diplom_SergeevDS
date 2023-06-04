using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metroidvania.BaseUnit
{
    public class UnitStats : UnitComponent
    {
        public event Action OnHealthZero;
        public event Action OnDecreaseHealth;
        [Header("Major stats")] 
        public Stats strength;
        public Stats agility;
        public Stats vitality;

        [Header("Defensive stats")] 
        public Stats maxHealth;
        public Stats armor;
        public Stats evasion;

        [Header("Offencive stats")] 
        public Stats critChance;
        public Stats critPower;
        
        [SerializeField] private int currentHealth;
        
        public int DoDamage(int baseDamage)
        {
            var finalDamage = strength.GetValue() + baseDamage;
            return CanCrit() ? CalculateCrt(finalDamage) : finalDamage;
        }
        public int ArrowDamage()
        {
            return agility.GetValue();
        }
        protected override void Awake()
        {
            base.Awake();
            currentHealth = GetMaxHealthValue();
        }
        protected override void Start()
        {
            base.Start();
            critPower.SetDefaultValue(150);
        }

        public void DecreaseHealth(int amount)
        {
            if(CanAvoidAttack()) return;
            currentHealth -= ArmorReduction(amount);
            OnDecreaseHealth?.Invoke();
            if (currentHealth >= 0) return;
            currentHealth = 0;
            OnHealthZero?.Invoke();
            Debug.Log("Health is 0!");
        }
        private bool CanAvoidAttack() =>Random.Range(0, 100) < evasion.GetValue() + agility.GetValue();
        private bool CanCrit() => Random.Range(0, 100) < critChance.GetValue() + agility.GetValue();
        private int CalculateCrt(int damage) =>Mathf.RoundToInt(damage * ((critPower.GetValue() + strength.GetValue()) * 0.1f));
        private int ArmorReduction(int damage) => Mathf.Clamp(damage - armor.GetValue(), 0, maxHealth.GetValue());
        public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;
        public int GetCurrentHealth() => currentHealth;
        public void IncreaseHealth(int amount) => currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth.GetValue());
    }
}



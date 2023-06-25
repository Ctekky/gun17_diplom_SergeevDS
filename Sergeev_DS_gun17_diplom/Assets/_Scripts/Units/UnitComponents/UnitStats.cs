using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metroidvania.BaseUnit
{
    public class UnitStats : UnitComponent
    {
        public event Action OnHealthZero;
        public event Action OnDecreaseHealth;
        public event Action<int> onHealthChange;
        [Header("Major stats")] public Stats strength;
        public Stats agility;
        public Stats vitality;

        [Header("Defensive stats")] public Stats health;
        public Stats armor;
        public Stats evasion;

        [Header("Offencive stats")] public Stats critChance;
        public Stats critPower;

        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;
        [SerializeField] private int unitLevel = 1;
        [SerializeField, Range(0f, 1f)] private float percentageModifier = 0.4f;

        public int UnitLevel
        {
            get => unitLevel;
            set => unitLevel = value;
        }
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
            ApplyLevelModifiers();
            GetMaxHealthValue();
            RestoreHealth();
        }

        private void ApplyLevelModifiers()
        {
            Modify(strength);
            Modify(agility);
            Modify(vitality);
        }
        private void Modify(Stats stat)
        {
            for (var i = 1; i < unitLevel; i++)
            {
                var modifier = stat.GetValue() * percentageModifier;
                stat.AddModifier(Mathf.RoundToInt(modifier));
            }
        }
        public int GetStat(StatType statType)
        {
            return statType switch
            {
                StatType.Strength => strength.GetValue(),
                StatType.Agility => agility.GetValue(),
                StatType.Vitality => vitality.GetValue(),
                StatType.Armor => armor.GetValue(),
                StatType.Evasion => evasion.GetValue(),
                StatType.CritChance => critChance.GetValue(),
                StatType.CritPower => critPower.GetValue(),
                StatType.Health => maxHealth,
                StatType.CurrentHealth => currentHealth,
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
        }
        public void DecreaseHealth(int amount)
        {
            if (currentHealth == 0)
            {
                OnHealthZero?.Invoke();
                return;
            }
            if (CanAvoidAttack()) return;
            currentHealth -= ArmorReduction(amount);
            OnDecreaseHealth?.Invoke();
            onHealthChange?.Invoke(currentHealth);
            if (currentHealth >= 0) return;
            currentHealth = 0;
            OnHealthZero?.Invoke();
        }
        private bool CanAvoidAttack() => Random.Range(0, 100) < evasion.GetValue() + agility.GetValue();
        private bool CanCrit() => Random.Range(0, 100) < critChance.GetValue() + agility.GetValue();
        private int CalculateCrt(int damage) =>
            Mathf.RoundToInt(damage * ((critPower.GetValue() + strength.GetValue()) * 0.1f));
        private int ArmorReduction(int damage) => Mathf.Clamp(damage - armor.GetValue(), 0, health.GetValue());
        public int GetMaxHealthValue()
        {
            maxHealth = health.GetValue() + vitality.GetValue() * 5;
            return maxHealth;
        }
        public int GetCurrentHealth() => currentHealth;
        public void SetCurrentHealth(int amount)
        {
            currentHealth = amount;
            GetMaxHealthValue();
            onHealthChange?.Invoke(currentHealth);
        }
        public void RestoreHealth()
        {
            currentHealth = maxHealth;
            onHealthChange?.Invoke(currentHealth);
        }
        public void IncreaseHealth(int amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            onHealthChange?.Invoke(currentHealth);
        }
    }
}
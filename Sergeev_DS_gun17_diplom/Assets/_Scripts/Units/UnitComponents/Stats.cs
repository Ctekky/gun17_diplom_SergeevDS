using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Metroidvania.BaseUnit
{
    [System.Serializable]
    public class Stats
    {
        [SerializeField] private int baseValue;
        public List<int> modifiers;

        public int GetValue()
        {
            return baseValue + modifiers.ToList().Sum();
        }
        public void SetDefaultValue(int value)
        {
            baseValue = value;
        }
        public void AddModifier(int value)
        {
            modifiers.Add(value);
        }
        public void RemoveModifier(int value)
        {
            modifiers.Remove(value);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Metroidvania.BaseUnit
{
    public class Unit : MonoBehaviour
    {
        private readonly List<UnitComponent> unitComponents = new List<UnitComponent>();

        private void Awake()
        {
        }
        public void LogicUpdate()
        {
            foreach (UnitComponent component in unitComponents)
            {
                component.LogicUpdate();
            }
        }
        public void AddComponent(UnitComponent unitComponent)
        {
            if (!unitComponents.Contains(unitComponent)) unitComponents.Add(unitComponent);
        }
        public T GetUnitComponent<T>() where T : UnitComponent
        {
            var component = unitComponents.OfType<T>().FirstOrDefault();
            if (component) return component;
            component = GetComponentInChildren<T>();
            if (component) return component;
            Debug.LogWarning($"{typeof(T)} not found in {transform.parent.name}");
            return null;
        }
        public T GetUnitComponent<T>(ref T value) where T : UnitComponent
        {
            value = GetUnitComponent<T>();
            return value;
        }
    }

}

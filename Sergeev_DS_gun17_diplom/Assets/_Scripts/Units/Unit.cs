using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Metroidvania.BaseUnit
{
    public class Unit : MonoBehaviour
    {
        private readonly List<UnitComponent> _unitComponents = new List<UnitComponent>();
        public void LogicUpdate()
        {
            foreach (UnitComponent component in _unitComponents)
            {
                component.LogicUpdate();
            }
        }
        public void AddComponent(UnitComponent unitComponent)
        {
            if (!_unitComponents.Contains(unitComponent)) _unitComponents.Add(unitComponent);
        }
        public T GetUnitComponent<T>() where T : UnitComponent
        {
            var component = _unitComponents.OfType<T>().FirstOrDefault();
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

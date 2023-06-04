using Metroidvania.Interfaces;
using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class UnitComponent : MonoBehaviour, ILogicUpdate
    {
        protected Unit Unit;
        public virtual void LogicUpdate() { }
        protected virtual void Awake()
        {
            Unit = transform.parent.GetComponent<Unit>();
            if (!Unit) Debug.LogError("No Unit script on parent");
            Unit.AddComponent(this);
        }
        protected virtual void Start() { }
    }
}


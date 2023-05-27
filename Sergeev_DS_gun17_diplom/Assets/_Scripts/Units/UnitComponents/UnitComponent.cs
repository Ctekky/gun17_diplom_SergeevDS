using Metroidvania.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class UnitComponent : MonoBehaviour, ILogicUpdate
    {
        protected Unit unit;

        public virtual void LogicUpdate() { }

        protected virtual void Awake()
        {
            unit = transform.parent.GetComponent<Unit>();
            if (!unit) Debug.LogError("No Unit script on parent");
            unit.AddComponent(this);

        }
        protected virtual void Start()
        {

        }
    }
}


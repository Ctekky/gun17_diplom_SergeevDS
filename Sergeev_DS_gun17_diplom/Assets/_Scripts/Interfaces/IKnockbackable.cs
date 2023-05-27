using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Interfaces
{
    public interface IKnockbackable 
    {
        void Knockback(Vector2 angle, float strength, int direction);
    }
}


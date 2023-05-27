using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Generic
{
    public static class GenericCheckForNullError<T>
    {
        public static T TryGet(T value, string name)
        {
            if (value != null) return value;
            Debug.LogError(typeof(T) + " return null on " + name);
            return default;
        }
    }
}



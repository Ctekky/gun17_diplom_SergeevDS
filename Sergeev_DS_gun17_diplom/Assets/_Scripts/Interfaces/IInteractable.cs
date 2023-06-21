using System;
using UnityEngine;

namespace Metroidvania.Interfaces
{
    public interface IInteractable
    {
        void Interact();
        void OnTriggerEnter2D(Collider2D other);
        void OnTriggerExit2D(Collider2D other);
        bool ReturnState();
        event Action<LootType, Vector2> Opened;
        event Action<Vector2> Used;
        event Action<Transform> Saved;
    }
    
}

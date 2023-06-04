using UnityEngine;
using Zenject;

namespace Metroidvania.Managers
{
    public class ItemManager : MonoBehaviour
    {
        [Inject] private Player.Player _player;
        [SerializeField] private GameObject itemPrefab; //for test

        public void CreateItem(Vector2 coordinates, GameObject itemToCreate)
        {
            var item = Instantiate(itemToCreate, coordinates, Quaternion.identity);
            
        }

    }
}


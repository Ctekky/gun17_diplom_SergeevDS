using System;
using UnityEditor;
using UnityEngine;

namespace Metroidvania.Common.Items
{
    [CreateAssetMenu(fileName = "Base Item", menuName = "Data/Item data/Base item")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public ItemType itemType;
        public string itemDescription;
        public string itemID;

        private void OnValidate()
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(this);
            itemID = AssetDatabase.AssetPathToGUID(path);
#endif


        }
    }
}
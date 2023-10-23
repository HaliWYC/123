using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{


    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;
        private Transform itemParent;

        private void Start()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }

        private void OnEnable()
        {
            EventHandler.instantiateItemInScene += onInstantiateItemInScene;
        }

        private void onInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }

        private void OnDisable()
        {
            EventHandler.instantiateItemInScene -= onInstantiateItemInScene;
        }
    }
}

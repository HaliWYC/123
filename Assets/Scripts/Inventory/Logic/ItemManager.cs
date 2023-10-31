using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShanHai_IsolatedCity.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;
        private Transform itemParent;

        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();

        private void OnEnable()
        {
            EventHandler.instantiateItemInScene += onInstantiateItemInScene;
            EventHandler.beforeSceneUnloadEvent += onBeforeSceneUnloadEvent;
            EventHandler.afterSceneLoadedEvent += onAfterSceneLoadEvent;
        }

        private void onAfterSceneLoadEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            recreateAllItems();
        }

        private void onInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }

        private void OnDisable()
        {
            EventHandler.instantiateItemInScene -= onInstantiateItemInScene;
            EventHandler.beforeSceneUnloadEvent += onBeforeSceneUnloadEvent;
            EventHandler.afterSceneLoadedEvent += onAfterSceneLoadEvent;
        }

        private void onBeforeSceneUnloadEvent()
        {
            getAllSceneItems();
        }

        /// <summary>
        /// Get all the items in scene
        /// </summary>
        private void getAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach(var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position = new SerilazableVector3(item.transform.position)
                };

                currentSceneItems.Add(sceneItem);
            }

            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //Find the data and update it
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else //If is new scene
            {
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }
        /// <summary>
        /// Refrash the item and recreate item
        /// </summary>
        private void recreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                    //Clean
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
    }
}

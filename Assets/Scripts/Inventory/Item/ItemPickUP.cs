using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{

    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item item = collision.GetComponent<Item>();

            if (item != null)
            {
                if (item.itemDetails.canPickUp)
                {
                    //PickUp Item to Bag

                    InventoryManager.Instance.AddItem(item);
                    EventHandler.CallUpdateTaskProgressEvent(item.itemDetails.itemName, 1);
                    Destroy(gameObject);
                }
            }
        }
    }
}
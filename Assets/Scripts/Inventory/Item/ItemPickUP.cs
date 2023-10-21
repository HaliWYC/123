using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{

    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();

            if (item != null)
            {
                if (item.itemDetails.canPickUp)
                {
                    //PickUp Item to Bag

                   InventoryManager.Instance.addItem(item, true);
                }
            }
        }
    }
}
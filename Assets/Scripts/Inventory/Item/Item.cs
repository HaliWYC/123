using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;
        public ItemType itemType;

        private SpriteRenderer spriteRenderer;

        private BoxCollider2D boxCollider2D;

        public ItemDetails itemDetails;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (itemID != 0)
            {
                Init(itemID,itemType);
            }
        }

        /// <summary>
        /// Initiate item on world
        /// </summary>
        /// <param name="ID"></param>
        public void Init(int ID,ItemType ItemType)
        {
            
            itemID = ID;
            itemType = ItemType;

            //Get data from inventory manager

            itemDetails = InventoryManager.Instance.GetItemDetails(itemID, itemType);
            
            if (itemDetails != null)
            {
                spriteRenderer.sprite = itemDetails.iconOnWorldSprite != null ? itemDetails.iconOnWorldSprite : itemDetails.itemIcon;
                
                //Debug.Log(spriteRenderer.sprite);
                // Change the collider size

                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
                boxCollider2D.size = newSize;
                boxCollider2D.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            }
        }
    }
}
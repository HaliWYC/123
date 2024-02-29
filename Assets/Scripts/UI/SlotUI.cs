using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace ShanHai_IsolatedCity.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        [Header("GetComponent")]

        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHighLight;
        [SerializeField] private Button button;

        [Header("SlotType")]
        public SlotType slotType;

        public bool isSelected;


        public int slotIndex;

        //Item Details

        public ItemDetails itemDetails;

        public int itemAmount;

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>(); 





        private void Start()
        {
            isSelected = false;

            if (itemDetails == null)
            {
                UpDateEmptySlot();
            }
        }



        /// <summary>
        /// Update Slot UI and information
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">HoldingNumber </param>
        public void UpDateSlot(ItemDetails item, int amount)
        {
            //Debug.Log(amount);
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            slotImage.enabled = true;
            itemAmount = amount;
            amountText.text = itemAmount.ToString();
            button.interactable = true;
        }


        /// <summary>
        /// Make the Slot to become empty
        /// </summary>
        public void UpDateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
                inventoryUI.UpDateSlotHighLight(-1);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
            itemDetails = null;
            itemAmount = 0;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails==null)
            {
                return;
            }
            isSelected = !isSelected;

            slotHighLight.gameObject.SetActive(isSelected);

            inventoryUI.UpDateSlotHighLight(slotIndex);

            if (slotType == SlotType.人物背包)
            {
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                //Debug.Log(itemAmount);
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();
                isSelected = true;
                inventoryUI.UpDateSlotHighLight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;

            if(eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                {
                    return;
                }

                var targetSlotUI = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                
                int targetIndex = targetSlotUI.slotIndex;

                //Swap item in player's bag
                if(slotType==SlotType.人物背包 && targetSlotUI.slotType == SlotType.人物背包)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
                else if(slotType == SlotType.NPC背包 && targetSlotUI.slotType == SlotType.人物背包)//Buy
                {
                    EventHandler.CallShowTradeUI(itemDetails, false, itemDetails.itemType);
                }
                else if(slotType == SlotType.人物背包 && targetSlotUI.slotType == SlotType.NPC背包)//Sell
                {
                    EventHandler.CallShowTradeUI(itemDetails, true, itemDetails.itemType);
                }

                //Clean all the highlight
                inventoryUI.UpDateSlotHighLight(-1);
            }
            /*else//Test for drop in the world
            {
                if (itemDetails.canDrop)
                {
                    var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                    EventHandler.callInstantiateItemInScene(itemDetails.itemID, pos);
                }
            }*/
        }
    }
}
 
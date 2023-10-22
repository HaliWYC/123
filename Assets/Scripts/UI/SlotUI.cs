using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace ShanHai_IsolatedCity.Inventory
{


    public class SlotUI : MonoBehaviour,IPointerClickHandler
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

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>(); 





        private void Start()
        {
            isSelected = false;

            if (itemDetails.itemID == 0)
            {
                upDateEmptySlot();
            }
        }



        /// <summary>
        /// Update Slot UI and information
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">HoldingNumber </param>
        public void upDateSlot(ItemDetails item, int amount)
        {
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
        public void upDateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0)
            {
                return;
            }
            isSelected = !isSelected;

            slotHighLight.gameObject.SetActive(isSelected);

            inventoryUI.upDateSlotHighLight(slotIndex); 
        }
    }
}
 
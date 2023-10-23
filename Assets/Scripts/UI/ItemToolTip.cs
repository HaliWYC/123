using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI typeText;

    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Text valueText;

    [SerializeField] private GameObject buttomPart;

    public void setUpToolTip(ItemDetails itemDetails,SlotType slotType)
    {
        nameText.text = itemDetails.itemName;

        typeText.text = "物品类型-" + itemDetails.itemType.ToString();

        descriptionText.text = itemDetails.itemDescription;

        if(itemDetails.itemType == ItemType.武器 || itemDetails.itemType == ItemType.商品 || itemDetails.itemType == ItemType.丹药)
        {
            buttomPart.SetActive(true);

            var price = itemDetails.itemPrice;
            

            if (slotType == SlotType.人物背包)
            {
                price = (int)(price * itemDetails.sellPercentage);
                
            }

            valueText.text = price.ToString();
            

        }
        else
        {
            buttomPart.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}

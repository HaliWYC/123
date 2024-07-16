using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;
using UnityEngine.EventSystems;

public class FavouriteItemSlotUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] private Image qualityIcon;
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject Disable;
    [SerializeField] private Text amount;
    [SerializeField] private ItemDetails itemDetail;
    private float Extent;

    private Vector3 mousePos;

    public void UpdateSlot(ItemDetails item, int itemAmount, float extent)
    {
        itemDetail = item;
        qualityIcon.color = item.itemType switch
        {
            ItemType.Equip => InventoryManager.Instance.GetEquipQualityColor(InventoryManager.Instance.GetEquipItemDetails(itemDetail.itemID).EquipItemQuality),
            ItemType.Consume => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetConsumeItemDetails(itemDetail.itemID).ConsumeItemQuality),
            ItemType.Task => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetTaskItemDetails(itemDetail.itemID).TaskItemQuality),
            ItemType.Other => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetOtherItemDetails(itemDetail.itemID).OtherItemQuality),
            _ => Color.white
        };
        itemIcon.sprite = itemDetail.itemIcon;

        if (itemAmount == 0)
        {
            amount.text = string.Empty;
            Disable.SetActive(true);
        }
        else
            amount.text = itemAmount.ToString();
        Extent = extent;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemDetail != null)
        {
            InventoryManager.Instance.inventoryUI.itemToolTip.SetUpItemToolTip(itemDetail);
            UpdateToolTipPosition(InventoryManager.Instance.inventoryUI.itemToolTip.GetComponent<RectTransform>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.inventoryUI.itemToolTip.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            GiftUI.Instance.GiveItems(itemDetail, Extent);
        }
    }

    private void UpdateToolTipPosition(RectTransform pos)
    {
        mousePos = Input.mousePosition;
        Vector3[] corners = new Vector3[4];

        pos.GetWorldCorners(corners);

        float height = corners[1].y - corners[0].y;
        float width = corners[3].x - corners[0].x;

        pos.pivot = new Vector2(-0.1f, 0.5f);
        if (Screen.height - mousePos.y >= height)
        {
            pos.position = mousePos + 0.5f * height * Vector3.up;
            if (Screen.width - mousePos.x >= 1.3 * width)
                pos.position = mousePos + 0.2f * width * Vector3.right;
            else
                pos.position = mousePos + 1.2f * width * Vector3.left;
        }
        else
        {
            pos.position = mousePos + 0.5f * height * Vector3.down;
            if (Screen.width - mousePos.x >= 1.3 * width)
                pos.position = mousePos + 0.2f * width * Vector3.right;
            else
                pos.position = mousePos + 1.2f * width * Vector3.left;
        }

        InventoryManager.Instance.inventoryUI.itemToolTip.gameObject.SetActive(true);
    }

    
}

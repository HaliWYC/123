using System;
using ShanHai_IsolatedCity.Inventory;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UseConsumeItemPanel : MonoBehaviour,IDragHandler
{
    private ConsumeItemDetails consumeItem;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image quality;
    [SerializeField] private Text itemName;
    [SerializeField] private InputField input;
    [SerializeField] private Button Confirm;
    [SerializeField] private Button Cancel;

    private void Awake()
    {
        Confirm.onClick.AddListener(ConfirmUseItem);
        Cancel.onClick.AddListener(CancelUI);
    }

    public void SetupConsumePanel(ConsumeItemDetails consume)
    {
        consumeItem = consume;
        itemName.text = consumeItem.itemName;
        itemIcon.sprite = consumeItem.itemIcon;
        quality.color = InventoryManager.Instance.GetQualityColor(consumeItem.ConsumeItemQuality);

    }

    private void ConfirmUseItem()
    {
        var amount = Convert.ToInt32(input.text);
        if (amount > 0)
            InventoryManager.Instance.UseConsumeItem(consumeItem,amount);
        else
            Debug.Log("Invalid Number, please try again");
        CancelUI();
    }

    private void CancelUI()
    {
        gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position = Input.mousePosition;
    }
}

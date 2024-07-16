using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;

public class GiftUI : Singleton<GiftUI>
{
    public GameObject giftPanel;
    public GameObject relationHolder;
    public GameObject favourItemHolder;
    public GameObject favourItemSlot;
    public GameObject personalityHolder;
    public GameObject personalitySlotPrefab;
    public Button leaveButton;

    private CharacterInformation_SO NPCInfor;
    public NPCDetails NPCDetail;
    [SerializeField] private Text NPCName;
    [SerializeField] private Image NPCIcon;
    [SerializeField] private Text NPCPrestige;
    [SerializeField] private Text Fitness;
    [SerializeField] private Text Eloquence;
    [SerializeField] private Text Wisdom;
    [SerializeField] private Text Luck;
    [SerializeField] private Text Strength;
    [SerializeField] private Text Perception;
    [SerializeField] private Text Relationship;

    protected override void Awake()
    {
        base.Awake();
        leaveButton.onClick.AddListener(Leave);
    }

    public void SetUpGiftUI(CharacterInformation_SO NPCInformation, NPCDetails npc)
    {
        NPCInfor = NPCInformation;
        NPCDetail = npc;
        NPCName.text = NPCInfor.Name;
        NPCIcon.sprite = NPCInfor.charIcon;
        NPCPrestige.text = NPCInfor.prestigeLevel.ToString();
        SetUpPersonalityPanel(NPCInfor.Personality);
        Fitness.text = NPCInfor.Fitness.ToString();
        Eloquence.text = NPCInfor.Eloquence.ToString();
        Wisdom.text = NPCInfor.Wisdom.ToString();
        Luck.text = NPCInfor.Luck.ToString();
        Strength.text = NPCInfor.Strength.ToString();
        Perception.text = NPCInfor.Perception.ToString();
        Relationship.text = NPCDetail.relationship.Favourability.ToString();
        Relationship.color = SwitchRelationshipColor(NPCDetail.relationship.Favourability);
        SetUpNPCFavouriteItems();
        giftPanel.SetActive(true);
    }

    private Color SwitchRelationshipColor(float amount)
    {
        Color Color;
        for (int index = 0; index < relationHolder.transform.childCount; index++)
        {
            relationHolder.transform.GetChild(index).gameObject.SetActive(false);
        }
        switch (amount)
        {
            case -100://Black
                Color = Color.black;
                relationHolder.transform.GetChild(8).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(8).gameObject.SetActive(true);
                return Color;
            case <-50://Black
                Color = Color.black;
                relationHolder.transform.GetChild(7).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(7).gameObject.SetActive(true);
                return Color;
            case <0://Grey
                Color = InventoryManager.Instance.GetBasicQualityColor(BasicQualityType.灰);
                relationHolder.transform.GetChild(6).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(6).gameObject.SetActive(true);
                return Color;
            case < 20://White
                Color = Color.white;
                relationHolder.transform.GetChild(5).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(5).gameObject.SetActive(true);
                return Color;
            case < 40://Blue
                Color = InventoryManager.Instance.GetBasicQualityColor(BasicQualityType.蓝);
                relationHolder.transform.GetChild(4).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(4).gameObject.SetActive(true);
                return Color;
            case < 60://Green
                Color = InventoryManager.Instance.GetBasicQualityColor(BasicQualityType.绿);
                relationHolder.transform.GetChild(3).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(3).gameObject.SetActive(true);
                return Color;
            case < 80://Yellow
                Color = InventoryManager.Instance.GetBasicQualityColor(BasicQualityType.黄);
                relationHolder.transform.GetChild(2).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(2).gameObject.SetActive(true);
                return Color;
            case < 100://Orange
                Color = InventoryManager.Instance.GetBasicQualityColor(BasicQualityType.橙);
                relationHolder.transform.GetChild(1).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(1).gameObject.SetActive(true);
                return Color;
            case 100://Red
                Color = Color.red;
                relationHolder.transform.GetChild(0).GetComponent<Text>().color = Color;
                relationHolder.transform.GetChild(0).gameObject.SetActive(true);
                return Color;
                

        }
        return Color.black;
    }

    private void SetUpPersonalityPanel(List<Personality> personalities)
    {
        if (personalityHolder.transform.childCount > 0)
        {
            for (int index = 0; index < personalityHolder.transform.childCount; index++)
            {
                Destroy(personalityHolder.transform.GetChild(index).gameObject);
            }
        }
        
        foreach(var person in personalities)
        {
            var personality = Instantiate(personalitySlotPrefab, personalityHolder.transform).GetComponent<PersonalitySlotUI>();
            personality.UpdateSlot(person);
        }
    }

    public void SetUpNPCFavouriteItems()
    {
        if (favourItemHolder.transform.childCount > 0)
        {
            for (int index = 0; index < favourItemHolder.transform.childCount; index++)
            {
                Destroy(favourItemHolder.transform.GetChild(index).gameObject);
            }
        }
        foreach (var fitem in NPCDetail.favouriteItems)
        {
            var fItem = Instantiate(favourItemSlot, favourItemHolder.transform).GetComponent<FavouriteItemSlotUI>();
            ItemDetails item = InventoryManager.Instance.GetItemDetails(fitem.itemID, fitem.itemType);
            fItem.UpdateSlot(item, InventoryManager.Instance.GetItemInBag(item), fitem.extent);
        }
        Relationship.text = NPCDetail.relationship.Favourability.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate(favourItemHolder.GetComponent<RectTransform>());
    }

    public float GiftCalculation(int quality, float extent)
    {
        float value = 0;
        //TODO:Execute buff
        switch (quality)
        {
            case 0:
                value = 5;
                break;
            case 1:
                value = 10;
                break;
            case 2:
                value = 20;
                break;
            case 3:
                value = 30;
                break;
            case 4:
                value = 50;
                break;
            case 5:
                value = 70;
                break;
            case 6:
                value = 90;
                break;
            case 7:
                value = 100;
                break;
        }
        if (extent < 0)
        {
            return (float)(value * (extent * -0.005 + 1));
        }
        else
            return (float)(value * (extent * 0.01 + 1));
    }

    public void GiveItems(ItemDetails itemDetail, float Extent)
    {
        if (NPCDetail != null)
        {
            InventoryManager.Instance.RemoveItem(itemDetail.itemID, itemDetail.itemType, 1);
            InventoryManager.Instance.AddItem(itemDetail, 1, NPCDetail, false);
            int quality = itemDetail.itemType switch
            {
                ItemType.Equip => (int)InventoryManager.Instance.GetEquipItemDetails(itemDetail.itemID).EquipItemQuality,
                ItemType.Consume => (int)InventoryManager.Instance.GetConsumeItemDetails(itemDetail.itemID).ConsumeItemQuality,
                ItemType.Task => (int)InventoryManager.Instance.GetTaskItemDetails(itemDetail.itemID).TaskItemQuality,
                _ => (int)InventoryManager.Instance.GetOtherItemDetails(itemDetail.itemID).OtherItemQuality,
            };
            NPCDetail.relationship.Favourability = Mathf.Min(NPCDetail.relationship.Favourability + GiftCalculation(quality, Extent), 100);
            Relationship.color = SwitchRelationshipColor(NPCDetail.relationship.Favourability);
            SetUpNPCFavouriteItems();
        }
    }

    private void Leave()
    {
        giftPanel.SetActive(false);
        EventHandler.CallNPCAvailableEvent(true);
    }
}

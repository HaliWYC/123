using UnityEngine;
using ShanHai_IsolatedCity.Inventory;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class TradeUI : Singleton<TradeUI>
{
    [Header("Component")]
    public GameObject TradePanel;
    public ItemTip itemTip;
    public Button TradeButton;
    public Button LeaveButton;
    public bool isSelected = false;

    [Header("Player")]
    [SerializeField] private Image PlayerIcon;
    [SerializeField] private Text PlayerName;
    [SerializeField] private Text PlayerPrestigeLevel;
    [SerializeField] private Text PlayerSHG;
    [SerializeField] private Text PlayerGold;
    private CharacterInformation playerInformation;
    private InventoryBag_SO playerBag;
    public GameObject playerBagSlot;
    public List<TradeItemSlot> playerBagSlots;

    [Header("NPC")]
    [SerializeField] private Image NPCIcon;
    [SerializeField] private Text NPCName;
    [SerializeField] private Text NPCPrestigeLevel;
    [SerializeField] private Text NPCSHG;
    [SerializeField] private Text NPCGold;
    private CharacterInformation NPCInformation;
    private NPCDetails NPCDetail;
    private InventoryBag_SO NPCBag;
    public GameObject NPCBagSlot;
    public List<TradeItemSlot> NPCBagSlots;

    [Header("Trade")]
    public GameObject TradeBagSlot;
    [SerializeField] private List<TradeItemSlot> TradeBagSlots;
    [SerializeField] private GameObject TradeItemSlotPrefab;
    [SerializeField] private Text relationShip;
    public Text TotalSHG;
    public Text TotalGold;
    public Button SubmmitButton;
    public Button CancelButton;
    public InputField amountInput;

    [Header("Submmition")]
    public GameObject submitPanel;
    [SerializeField] private TradeItemSlot SubmitItem;
    private TradeItemSlot selectedItem;
    [SerializeField] private Text itemMainType;
    [SerializeField] private Text itemSubType;


    protected override void Awake()
    {
        base.Awake();
        TradeButton.onClick.AddListener(Trade);
        LeaveButton.onClick.AddListener(Leave);
        SubmmitButton.onClick.AddListener(Submit);
        CancelButton.onClick.AddListener(Cancel);
    }
    private void OnEnable()
    {
        EventHandler.CheckInvalidItemsEvent += OnCheckInvalidItemsEvent;
    }

    private void OnDisable()
    {
        EventHandler.CheckInvalidItemsEvent -= OnCheckInvalidItemsEvent;
    }

    private void OnCheckInvalidItemsEvent()
    {
        if (NPCBag != null)
        {
            foreach (var itemInbag in NPCBag.equipList)
            {
                if (InventoryManager.Instance.GetItemDetails(itemInbag.itemID, itemInbag.Type) == null)
                    NPCBag.equipList.Remove(itemInbag);
            }
            foreach (var itemInbag in NPCBag.consumeList)
            {
                if (InventoryManager.Instance.GetItemDetails(itemInbag.itemID, itemInbag.Type) == null)
                    NPCBag.consumeList.Remove(itemInbag);
            }
            foreach (var itemInbag in NPCBag.taskList)
            {
                if (InventoryManager.Instance.GetItemDetails(itemInbag.itemID, itemInbag.Type) == null)
                    NPCBag.taskList.Remove(itemInbag);
            }
            foreach (var itemInbag in NPCBag.otherList)
            {
                if (InventoryManager.Instance.GetItemDetails(itemInbag.itemID, itemInbag.Type) == null)
                    NPCBag.otherList.Remove(itemInbag);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isSelected)
            {
                itemTip.gameObject.SetActive(!itemTip.gameObject.activeInHierarchy);
            }
            else
                itemTip.gameObject.SetActive(false);
        }
    }

    public void SetUpTraderBag(CharacterInformation NPCInfor,NPCDetails NPCDetails,InventoryBag_SO player)
    {
        playerInformation = GameManager.Instance.playerInformation;
        playerBag = player;
        NPCInformation = NPCInfor;
        NPCDetail = NPCDetails;
        NPCBag = NPCDetail.NPCBag;
        UpdatePlayer();
        UpdateNPC();
        RefreshTradeBag();
        TotalSHG.text = "0";
        TotalGold.text = "0";
        TradePanel.SetActive(true);
    }

    private void UpdatePlayer()
    {
        PlayerIcon.sprite = playerInformation.characterInformation.charIcon;
        PlayerName.text = playerInformation.characterInformation.Name;
        PlayerPrestigeLevel.text = playerInformation.characterInformation.prestigeLevel.ToString();
        PlayerSHG.text = playerBag.ShanHaiGold.ToString();
        PlayerGold.text = playerBag.Gold.ToString();
        UpdatePlayerBagItems();
    }

    private void UpdateNPC()
    {
        NPCIcon.sprite = NPCInformation.characterInformation.charIcon;
        NPCName.text = NPCInformation.characterInformation.Name;
        NPCPrestigeLevel.text = NPCInformation.characterInformation.prestigeLevel.ToString();
        NPCSHG.text = NPCBag.ShanHaiGold.ToString();
        NPCGold.text = NPCBag.Gold.ToString();
        relationShip.text = NPCDetail.relationship.Favourability.ToString();
        UpdateNPCBagItems();
    }

    #region UpdateBagItem
    private void UpdatePlayerBagItems()
    {
        if (playerBagSlot.transform.childCount > 0)
        {
            for (int index = 0; index < playerBagSlot.transform.childCount; index++)
            {
                Destroy(playerBagSlot.transform.GetChild(index).gameObject);
            }
        }
        playerBagSlots.Clear();

        for (int i = 0; i < playerBag.equipList.Count; i++)
        {
            if (playerBag.equipList[i].itemAmount > 0)
            {
                var Item = Instantiate(TradeItemSlotPrefab, playerBagSlot.transform).GetComponent<TradeItemSlot>();
                playerBagSlots.Add(Item);
                Item.UpdateSlot(InventoryManager.Instance.GetEquipItemDetails(playerBag.equipList[i].itemID), playerBag.equipList[i].Type, playerBag.equipList[i].itemAmount, SlotType.PlayerBag);
            }
        }
        for (int i = 0; i < playerBag.consumeList.Count; i++)
        {
            if (playerBag.consumeList[i].itemAmount > 0)
            {
                var Item = Instantiate(TradeItemSlotPrefab, playerBagSlot.transform).GetComponent<TradeItemSlot>();
                playerBagSlots.Add(Item);
                Item.UpdateSlot(InventoryManager.Instance.GetConsumeItemDetails(playerBag.consumeList[i].itemID), playerBag.consumeList[i].Type, playerBag.consumeList[i].itemAmount, SlotType.PlayerBag);
            }
        }
        for (int i = 0; i < playerBag.taskList.Count; i++)
        {
            if (playerBag.taskList[i].itemAmount > 0)
            {
                var Item = Instantiate(TradeItemSlotPrefab, playerBagSlot.transform).GetComponent<TradeItemSlot>();
                playerBagSlots.Add(Item);
                Item.UpdateSlot(InventoryManager.Instance.GetTaskItemDetails(playerBag.taskList[i].itemID), playerBag.taskList[i].Type, playerBag.taskList[i].itemAmount, SlotType.PlayerBag);
            }
        }
        for (int i = 0; i < playerBag.otherList.Count; i++)
        {
            if (playerBag.otherList[i].itemAmount > 0)
            {
                var Item = Instantiate(TradeItemSlotPrefab, playerBagSlot.transform).GetComponent<TradeItemSlot>();
                playerBagSlots.Add(Item);
                Item.UpdateSlot(InventoryManager.Instance.GetOtherItemDetails(playerBag.otherList[i].itemID), playerBag.otherList[i].Type, playerBag.otherList[i].itemAmount, SlotType.PlayerBag);
            }
        }
    }

    public void UpdateNPCBagItems()
    {
        if (NPCBagSlot.transform.childCount > 0)
        {
            for (int index = 0; index < NPCBagSlot.transform.childCount; index++)
            {
                Destroy(NPCBagSlot.transform.GetChild(index).gameObject);
            }
        }
        NPCBagSlots.Clear();

        for (int i = 0; i < NPCBag.equipList.Count; i++)
        {
            if (NPCBag.equipList[i].itemAmount > 0)
            {
                var Item = Instantiate(TradeItemSlotPrefab, NPCBagSlot.transform).GetComponent<TradeItemSlot>();
                NPCBagSlots.Add(Item);
                Item.UpdateSlot(InventoryManager.Instance.GetEquipItemDetails(NPCBag.equipList[i].itemID), NPCBag.equipList[i].Type, NPCBag.equipList[i].itemAmount, SlotType.NPCBag);
            }
        }
        for (int i = 0; i < NPCBag.consumeList.Count; i++)
        {
            if (NPCBag.consumeList[i].itemAmount > 0)
            {
                var Item = Instantiate(TradeItemSlotPrefab, NPCBagSlot.transform).GetComponent<TradeItemSlot>();
                NPCBagSlots.Add(Item);
                Item.UpdateSlot(InventoryManager.Instance.GetConsumeItemDetails(NPCBag.consumeList[i].itemID), NPCBag.consumeList[i].Type, NPCBag.consumeList[i].itemAmount, SlotType.NPCBag);
            }
        }
        for (int i = 0; i < NPCBag.taskList.Count; i++)
        {
            if (NPCBag.taskList[i].itemAmount > 0)
            {
                var Item = Instantiate(TradeItemSlotPrefab, NPCBagSlot.transform).GetComponent<TradeItemSlot>();
                NPCBagSlots.Add(Item);
                Item.UpdateSlot(InventoryManager.Instance.GetTaskItemDetails(NPCBag.taskList[i].itemID), NPCBag.taskList[i].Type, NPCBag.taskList[i].itemAmount, SlotType.NPCBag);
            }
        }
        for (int i = 0; i < NPCBag.otherList.Count; i++)
        {
            if (NPCBag.otherList[i].itemAmount > 0)
            {
                var Item = Instantiate(TradeItemSlotPrefab, NPCBagSlot.transform).GetComponent<TradeItemSlot>();
                NPCBagSlots.Add(Item);
                Item.UpdateSlot(InventoryManager.Instance.GetOtherItemDetails(NPCBag.otherList[i].itemID), NPCBag.otherList[i].Type, NPCBag.otherList[i].itemAmount, SlotType.NPCBag);
            }
        }
    }

    /// <summary>
    /// Clean up the trade bag
    /// </summary>
    private void RefreshTradeBag()
    {
        if (TradeBagSlot.transform.childCount > 0)
        {
            for (int index = 0; index < TradeBagSlot.transform.childCount; index++)
            {
                Destroy(TradeBagSlot.transform.GetChild(index).gameObject);
            }
        }
        TradeBagSlots.Clear();
    }

    private void AddItemToTradeBag(ItemDetails item,ItemType itemType, int amount, SlotType location)
    {
        int index = GetItemIndexInTradeBag(item, location);
        if (index == -1)
        {
            var Item = Instantiate(TradeItemSlotPrefab, TradeBagSlot.transform).GetComponent<TradeItemSlot>();
            TradeBagSlots.Add(Item);
            Item.inTrade = true;
            Item.UpdateSlot(item, itemType, amount, location);
        }
        else
        {
            int totalAmount = amount += TradeBagSlots[index].itemAmount ;
            TradeBagSlots[index].UpdateSlot(item, itemType, totalAmount, location);
        }
    }

    private int GetItemIndexInTradeBag(ItemDetails item, SlotType location)
    {
        for(int index = 0; index < TradeBagSlots.Count; index++)
        {
            if (TradeBagSlots[index].itemDetails == item && TradeBagSlots[index].slotType == location)
                return index;
        }
        return -1;
    }


    public void UpdateSubmitionPanel(TradeItemSlot itemSlot)
    {
        selectedItem = itemSlot;
        SubmitItem.UpdateSlot(selectedItem.itemDetails, selectedItem.itemDetails.itemType, selectedItem.slotType);
        itemMainType.text = selectedItem.itemDetails.itemType.ToString();
        itemSubType.text = selectedItem.itemDetails.itemType
            switch
        {
            ItemType.Equip => InventoryManager.Instance.GetEquipItemDetails(selectedItem.itemDetails.itemID).equipItemType.ToString(),
            ItemType.Consume => InventoryManager.Instance.GetConsumeItemDetails(selectedItem.itemDetails.itemID).consumeItemType.ToString(),
            ItemType.Task => InventoryManager.Instance.GetTaskItemDetails(selectedItem.itemDetails.itemID).taskItemType.ToString(),
            _ => InventoryManager.Instance.GetOtherItemDetails(selectedItem.itemDetails.itemID).otherItemType.ToString()
        };
        submitPanel.SetActive(true);
    }

    /// <summary>
    /// Return the not traded items in the trade bag
    /// </summary>
    private void ReturnTradeItem()
    {
        if (TradeBagSlots.Count > 0)
        {
            foreach(var slot in TradeBagSlots)
            {
                if (slot.slotType == SlotType.PlayerBag)
                {
                    InventoryManager.Instance.AddItem(slot.itemDetails, slot.itemAmount);
                }
                else if (slot.slotType == SlotType.NPCBag)
                {
                    InventoryManager.Instance.AddItem(slot.itemDetails, slot.itemAmount, NPCDetail, true);
                }
            }
        }
        RefreshTradeBag();
    }

    private void CalculateTotalValue()
    {
        double SHG = 0;
        double Gold = 0;
        int relation = System.Convert.ToInt32(relationShip.text);

        foreach (var slot in TradeBagSlots)
        {
            if(slot.slotType == SlotType.PlayerBag)
            {
                SHG += slot.itemDetails.shanHaiGold * slot.itemAmount* RelationCorrection(relation, SlotType.PlayerBag);
                Gold += slot.itemDetails.gold * slot.itemAmount* RelationCorrection(relation, SlotType.PlayerBag);
            }
            else if (slot.slotType == SlotType.NPCBag)
            {
                SHG -= slot.itemDetails.shanHaiGold * slot.itemAmount* RelationCorrection(relation, SlotType.PlayerBag);
                Gold -= slot.itemDetails.gold * slot.itemAmount* RelationCorrection(relation, SlotType.PlayerBag);
            }
        }
        TotalSHG.DOText(((int)SHG).ToString(), 0.2f);
        TotalGold.DOText(((int)Gold).ToString(), 0.2f);
    }

    private double RelationCorrection(int relation, SlotType tradeType)
    {
        switch (tradeType)
        {
            case SlotType.PlayerBag:
                if (relation < 0)
                    return relation * 0.01 + 1;
                else
                    return relation * 0.006 + 1;
            case SlotType.NPCBag:
                if (relation < 0)
                    return relation * (-0.02) + 1;
                else
                    return relation * (-0.004) + 1;
        }
        return 1;
    }

    private bool CheckItemEnough(InventoryBag_SO bag, ItemDetails itemDetails, ItemType itemType, int amount)
    {
        if (itemDetails != null)
        {
            switch (itemType)
            {
                case ItemType.Equip:
                    if (bag.equipList.Find(i => i.itemID == itemDetails.itemID).itemAmount >= amount)
                        return true;
                    break;
                case ItemType.Consume:
                    if (bag.consumeList.Find(i => i.itemID == itemDetails.itemID).itemAmount >= amount)
                        return true;
                    break;
                case ItemType.Task:
                    if (bag.taskList.Find(i => i.itemID == itemDetails.itemID).itemAmount >= amount)
                        return true;
                    break;
                case ItemType.Other:
                    if (bag.otherList.Find(i => i.itemID == itemDetails.itemID).itemAmount >= amount)
                        return true;
                    break;
            }
        }
        return false;
    }

    private bool CheckEnoughSHG(int playerSHG, int NPCSHG)
    {
        int SHG = System.Convert.ToInt32(TotalSHG.text);
        if (SHG > 0)
        {
            if (SHG <= NPCSHG)
                return true;
        }
        else if (SHG == 0)
            return true;
        else
            if (playerSHG + SHG >= 0)
            return true;
        return false;
    }

    private bool CheckEnoughGold(int playerGold, int NPCGold)
    {
        int Gold = System.Convert.ToInt32(TotalGold.text);
        if (Gold > 0)
        {
            if (Gold <= NPCGold)
                return true;
        }
        else if (Gold == 0)
            return true;
        else
            if (playerGold + Gold >= 0)
            return true;
        return false;
    }

    private void TradeItems()
    {
        foreach(var slot in TradeBagSlots)
        {
            if(slot.slotType == SlotType.PlayerBag)
            {
                InventoryManager.Instance.AddItem(slot.itemDetails, slot.itemAmount, NPCDetail, true);
            }
            else if (slot.slotType == SlotType.NPCBag)
            {
                InventoryManager.Instance.AddItem(slot.itemDetails, slot.itemAmount);
            }
        }
        RefreshTradeBag();
    }

    private void UpdateMoney()
    {
        int SHG = System.Convert.ToInt32(TotalSHG.text);
        int Gold = System.Convert.ToInt32(TotalGold.text);

        NPCBag.ShanHaiGold -= SHG;
        playerBag.ShanHaiGold += SHG;
        NPCBag.Gold -= Gold;
        playerBag.Gold += Gold;
        CalculateTotalValue();

        PlayerSHG.DOText(playerBag.ShanHaiGold.ToString(), 0.2f);
        PlayerGold.DOText(playerBag.Gold.ToString(), 0.2f);
        NPCSHG.DOText(NPCBag.ShanHaiGold.ToString(), 0.2f);
        NPCGold.DOText(NPCBag.Gold.ToString(), 0.2f);

    }

    #endregion

    #region ButtonFuncitons
    private void Trade()
    {
        if (CheckEnoughSHG(playerBag.ShanHaiGold, NPCBag.ShanHaiGold) && CheckEnoughGold(playerBag.Gold, NPCBag.Gold))
        {
            TradeItems();
            UpdateMoney();
        }
    }
    private void Leave()
    {
        itemTip.gameObject.SetActive(false);
        TradePanel.SetActive(false);
        ReturnTradeItem();
        EventHandler.CallNPCAvailableEvent(true);
    }
    private void Submit()
    {
        int amount = 0; 
        if(amountInput.text!=string.Empty)
            amount = System.Convert.ToInt32(amountInput.text);
        if (selectedItem.inTrade)
        {
            switch (selectedItem.slotType)
            {
                case SlotType.PlayerBag:
                    if (amount < selectedItem.itemAmount)
                    {
                        TradeItemSlot newItem = TradeBagSlots.Find(i => i.itemDetails == selectedItem.itemDetails);
                        newItem.UpdateSlot(newItem.itemDetails, newItem.itemDetails.itemType, newItem.itemAmount - amount, SlotType.PlayerBag);
                        InventoryManager.Instance.AddItem(selectedItem.itemDetails, amount);
                    }
                    else if (amount == selectedItem.itemAmount)
                    {
                        InventoryManager.Instance.AddItem(selectedItem.itemDetails, amount);
                        for(int index = 0; index < TradeBagSlots.Count; index++)
                        {
                            if (TradeBagSlots[index].itemDetails == selectedItem.itemDetails)
                            {
                                Destroy(TradeBagSlots[index].gameObject);
                                TradeBagSlots.RemoveAt(index);
                            }
                        }
                    }
                    UpdatePlayerBagItems();
                    break;
                case SlotType.NPCBag:
                    if (amount < selectedItem.itemAmount)
                    {
                        TradeItemSlot newItem = TradeBagSlots.Find(i => i.itemDetails == selectedItem.itemDetails);
                        newItem.UpdateSlot(newItem.itemDetails, newItem.itemDetails.itemType, newItem.itemAmount - amount, SlotType.NPCBag);
                        InventoryManager.Instance.AddItem(selectedItem.itemDetails, amount, NPCDetail, true);
                    }
                    else if (amount == selectedItem.itemAmount)
                    {
                        InventoryManager.Instance.AddItem(selectedItem.itemDetails, amount, NPCDetail, true);
                        for (int index = 0; index < TradeBagSlots.Count; index++)
                        {
                            if (TradeBagSlots[index].itemDetails == selectedItem.itemDetails)
                            {
                                Destroy(TradeBagSlots[index].gameObject);
                                TradeBagSlots.RemoveAt(index);
                            }
                        }
                    }
                    UpdateNPCBagItems();
                    break;
            }
        }
        else
        {

            switch (selectedItem.slotType)
            {
                case SlotType.PlayerBag:
                    if(CheckItemEnough(playerBag,selectedItem.itemDetails, selectedItem.itemDetails.itemType, amount))
                    {
                        AddItemToTradeBag(selectedItem.itemDetails, selectedItem.itemDetails.itemType, amount, selectedItem.slotType);
                        InventoryManager.Instance.RemoveItem(selectedItem.itemDetails.itemID, selectedItem.itemDetails.itemType, amount);
                        UpdatePlayerBagItems();
                    }
                    break;
                case SlotType.NPCBag:
                    if (CheckItemEnough(NPCBag, selectedItem.itemDetails, selectedItem.itemDetails.itemType, amount))
                    {
                        AddItemToTradeBag(selectedItem.itemDetails, selectedItem.itemDetails.itemType, amount, selectedItem.slotType);
                        InventoryManager.Instance.RemoveItem(selectedItem.itemDetails.itemID, selectedItem.itemDetails.itemType, amount, NPCDetail);
                    }
                    break;
            }
        }
        submitPanel.SetActive(false);
        amountInput.text = string.Empty;
        CalculateTotalValue();
    }
    private void Cancel()
    {
        submitPanel.SetActive(false);
    }
    #endregion
}

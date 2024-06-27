using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor.UIElements;

public class ConsumeItemDetailsEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private VisualTreeAsset itemRowTemplate;

    private ConsumeItemDetailsList_SO database;

    //Get the Visual Element
    private ListView itemListView;

    private ScrollView itemDetailsSection;

    private ConsumeItemDetails activeItem;

    private VisualElement iconPreview;

    private Sprite defaultIcon;

    private List<ConsumeItemDetails> itemList = new List<ConsumeItemDetails>();

    [MenuItem("山海：绝命孤城/ConsumeItemDetailsEditor")]
    public static void ShowExample()
    {
        ConsumeItemDetailsEditor wnd = GetWindow<ConsumeItemDetailsEditor>();
        wnd.titleContent = new GUIContent("ConsumeItemDetailsEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
        //Get defaultIcon
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");
        //Variable assignments
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");


        //Get Buttons

        root.Q<Button>("AddButton").clicked += onAddItemClicked;
        root.Q<Button>("DeleteButton").clicked += onDeleteItemClicked;

        //Get the Template Data
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRowTemplate.uxml");

        //Load data
        loadDataBase();

        //Generate List View
        generateListView();
    }

    private void onAddItemClicked()
    {
        ConsumeItemDetails newItem = new ConsumeItemDetails();

        newItem.itemName = "NEW ITEM";
        newItem.itemID = itemList.Count;
        itemList.Add(newItem);

        itemListView.Rebuild();
    }

    private void onDeleteItemClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }

    private void loadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("ConsumeItemDetailsList_SO");

        if (dataArray.Length > 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            database = AssetDatabase.LoadAssetAtPath(path, typeof(ConsumeItemDetailsList_SO)) as ConsumeItemDetailsList_SO;
        }

        itemList = database.consumeItemDetailsList;

        //It will not store the data if it is not marked as dirty
        EditorUtility.SetDirty(database);
    }

    private void generateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < itemList.Count)
            {
                if (itemList[i].itemIcon != null)
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEM" : itemList[i].itemName;
            }

        };

        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;

        itemListView.selectionChanged += onListSelectionChange;

        itemDetailsSection.visible = false;
    }

    private void onListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ConsumeItemDetails)selectedItem.First();
        getItemDetails();
        itemDetailsSection.visible = true;
    }

    private void getItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();

        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID = evt.newValue;
        });

        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;

            itemListView.Rebuild();
        });

        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.iconOnWorldSprite;
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.iconOnWorldSprite = (Sprite)evt.newValue;

            itemListView.Rebuild();
        });

        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {

            activeItem.itemType = (ItemType)evt.newValue;
            itemListView.Rebuild();
        });

        itemDetailsSection.Q<EnumField>("ItemQuality").Init(activeItem.ConsumeItemQuality);
        itemDetailsSection.Q<EnumField>("ItemQuality").value = activeItem.ConsumeItemQuality;
        itemDetailsSection.Q<EnumField>("ItemQuality").RegisterValueChangedCallback(evt =>
        {

            activeItem.ConsumeItemQuality = (BasicQualityType)evt.newValue;
            itemListView.Rebuild();
        });

        itemDetailsSection.Q<EnumField>("ConsumeType").Init(activeItem.consumeItemType);
        itemDetailsSection.Q<EnumField>("ConsumeType").value = activeItem.consumeItemType;
        itemDetailsSection.Q<EnumField>("ConsumeType").RegisterValueChangedCallback(evt =>
        {

            activeItem.consumeItemType = (ConsumeItemType)evt.newValue;
            itemListView.Rebuild();
        });

        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemUseRadius").value = activeItem.useRadius;
        itemDetailsSection.Q<IntegerField>("ItemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.useRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanPickUp").value = activeItem.canPickUp;
        itemDetailsSection.Q<Toggle>("CanPickUp").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickUp = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("Stackable").value = activeItem.Stackable;
        itemDetailsSection.Q<Toggle>("Stackable").RegisterValueChangedCallback(evt =>
        {
            activeItem.Stackable = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanDrop").value = activeItem.canDrop;
        itemDetailsSection.Q<Toggle>("CanDrop").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDrop = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("notForSale").value = activeItem.notForSale;
        itemDetailsSection.Q<Toggle>("notForSale").RegisterValueChangedCallback(evt =>
        {
            activeItem.notForSale = evt.newValue;
        });

        itemDetailsSection.Q<ObjectField>("ConsumeData").value = activeItem.consumeData;
        itemDetailsSection.Q<ObjectField>("ConsumeData").RegisterValueChangedCallback(evt =>
        {
            activeItem.consumeData = (ConsumeItem_SO)evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("Gold").value = activeItem.gold;
        itemDetailsSection.Q<IntegerField>("Gold").RegisterValueChangedCallback(evt =>
        {
            activeItem.gold = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ShanHaiGold").value = activeItem.shanHaiGold;
        itemDetailsSection.Q<IntegerField>("ShanHaiGold").RegisterValueChangedCallback(evt =>
        {
            activeItem.shanHaiGold = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });
    }
}

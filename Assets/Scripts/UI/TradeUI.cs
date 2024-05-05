using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShanHai_IsolatedCity.Inventory
{
    public class TradeUI : MonoBehaviour
    {
        public Image itemIcon;
        public Text itemName;
        public Text qualityType;
        public Text totalPrice;
        public InputField tradeAmount;
        public Button submit;
        public Button cancel;


        private ItemDetails itemDetails;
        private bool isSellTrade;
        private bool isWeapon;


        private void Awake()
        {
            cancel.onClick.AddListener(cancleTradeUI);
            submit.onClick.AddListener(tradeItem);
        }

        public void SetUPTradeUI(ItemDetails item, bool isSell, ItemType itemType)
        {
            this.itemDetails = item;
            itemIcon.sprite = item.itemIcon;
            itemName.text = item.itemName;
            isSellTrade = isSell;
            tradeAmount.text = string.Empty;
            //FIXME 修改不同装备显示效果
            //qualityType.text = item.qualityType.ToString();
            qualityType.gameObject.SetActive(true);

            switch (itemType)
            {
                case ItemType.Equip:

                    break;
                case ItemType.Other:
                    break;
            }
            
            if (tradeAmount.text != string.Empty)
                totalPrice.text = (item.gold * Convert.ToInt32(tradeAmount.text)).ToString();
            else
                totalPrice.gameObject.SetActive(false);

        }

        public void tradeItem()
        {
            var amount = Convert.ToInt32(tradeAmount.text);
            //InventoryManager.Instance.TradeItem(itemDetails, amount, isSellTrade);
            cancleTradeUI(); 
        }

        public void cancleTradeUI()
        {
            this.gameObject.SetActive(false);
        }
    }

}

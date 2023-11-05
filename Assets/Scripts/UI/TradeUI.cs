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
        public Text weaponQuaity;
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

        public void setUPTradeUI(ItemDetails item, bool isSell)
        {
            this.itemDetails = item;
            itemIcon.sprite = item.itemIcon;
            itemName.text = item.itemName;
            isSellTrade = isSell;
            tradeAmount.text = string.Empty;
            qualityType.text = item.itemQuality.ToString();
            weaponQuaity.text = item.weaponQuality.ToString();

            isWeapon = item.weaponType switch { WeaponType.非武器 => false, _ => true };

            weaponQuaity.gameObject.SetActive(isWeapon);

            if (!isWeapon)
                qualityType.gameObject.SetActive(true);
            else
                qualityType.gameObject.SetActive(false);

            if (tradeAmount.text != string.Empty)
                totalPrice.text = (item.itemPrice * int.Parse(tradeAmount.text)).ToString();
            else
                totalPrice.gameObject.SetActive(false);



            //qualityType.text = item.itemQuality switch {
            //    QualityType.赤 => "赤",
            //    QualityType.橙 => "橙",
            //    QualityType.黄=>"黄",
            //    QualityType.绿=>"绿",
            //    QualityType.青=>"青",
            //    QualityType.蓝=>"蓝",
            //    QualityType.紫=>"紫",
            //    _=>"灰"
            //};


            //weaponQuaity.text = item.weaponQuality switch {
            //    WeaponQualityType.神器=>"神器",
            //    WeaponQualityType.传说=>"传说",
            //    WeaponQualityType.史诗=>"史诗",
            //    WeaponQualityType.卓越=>"卓越",
            //    WeaponQualityType.精良=>"精良",
            //    WeaponQualityType.优秀=>"优秀",
            //    WeaponQualityType.普通=>"普通",
            //    _=>"残品"
            //};

        }

        public void tradeItem()
        {
            var amount = Convert.ToInt32(tradeAmount.text);
            InventoryManager.Instance.tradeItem(itemDetails, amount, isSellTrade);
            cancleTradeUI(); 
        }

        public void cancleTradeUI()
        {
            this.gameObject.SetActive(false);
        }
    }

}

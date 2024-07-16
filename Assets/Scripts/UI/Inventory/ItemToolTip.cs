using System.Collections;
using System.Collections.Generic;
using ShanHai_IsolatedCity.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private Text itemName;
    [SerializeField] private Text Description;

    public void SetUpItemToolTip(ItemDetails item)
    {
        if (item != null)
        {
            itemName.text = item.itemName;
            Description.text = item.itemDescription;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipItemDetailsList_SO", menuName = "Inventory/EquipItemDetailsList")]
public class EquipItemDetailsList_SO : ScriptableObject
{
    public List<EquipItemDetails> equipItemDetailsList;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;

public class PersonalitySlotUI : MonoBehaviour
{
    private Personality personality;
    [SerializeField] private Text Name;
    [SerializeField] private Image qualityIcon;
    private string description;

    public void UpdateSlot(Personality person)
    {
        personality = person;
        Name.text = personality.Name;
        qualityIcon.color = InventoryManager.Instance.GetBasicQualityColor(personality.personalityQuality);
        description = personality.description;
    }
}

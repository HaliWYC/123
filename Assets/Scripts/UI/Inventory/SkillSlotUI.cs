using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;
using UnityEngine.EventSystems;

namespace ShanHai_IsolatedCity.Skill
{
    public class SkillSlotUI : MonoBehaviour,IPointerClickHandler
    {

        [SerializeField] private Image skillIcon;
        [SerializeField] private Image qualityIcon;
        [SerializeField] private Image Percentage;
        [SerializeField] private Text Name;
        [SerializeField] private Text Quality;
        [SerializeField] private Text Proficiency;
        [SerializeField] private Text ExpText;

        public SkillDetails_SO skillDetails;

        public bool isSkillDetailsOpen;
        

        public void UpdateSlot(SkillDetails_SO skill)
        {
            skillDetails = skill;
            skillIcon.sprite = skillDetails.skillIcon;
            qualityIcon.color = InventoryManager.Instance.GetBasicQualityColor(skillDetails.skillQuality);
            Percentage.fillAmount = skillDetails.currentExp / skillDetails.nextExp;
            Name.text = skillDetails.SkillName;
            Quality.text = skillDetails.skillQuality.ToString();
            Proficiency.text = skillDetails.skillProficiency.ToString();
            ExpText.text = skillDetails.currentExp.ToString() + " / " + skillDetails.nextExp.ToString();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (skillDetails != null)
            {
                InventoryManager.Instance.inventoryUI.equipItemTip.SetUpSkillToolTip(skillDetails);
                
            }
            if (isSkillDetailsOpen)
            {
                InventoryManager.Instance.inventoryUI.equipItemTip.ReGenerateSkillSlot();
                isSkillDetailsOpen = false;
            }

        }
    }
}


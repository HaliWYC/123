using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;
public class PlayerSkillSlot : MonoBehaviour
{
    private SkillDetails_SO skillDetail;
    [SerializeField] private Text buttonText;
    [SerializeField] private Button SkillButton;
    [SerializeField] private Text CoolDownText;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image quality;

    private void Update()
    {
        if (skillDetail != null)
        {
            if (skillDetail.currentCoolDown > 0)
            {
                CoolDownText.gameObject.SetActive(true);
                CoolDownText.text = skillDetail.currentCoolDown.ToString();
            }
            else
            {
                CoolDownText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateSlot(SkillDetails_SO skill, int index)
    {
        skillDetail = skill;
        buttonText.text = (index + 1).ToString();
        skillIcon.sprite = skill.skillIcon;
        quality.color = InventoryManager.Instance.GetBasicQualityColor(skill.skillQuality);
    }
}

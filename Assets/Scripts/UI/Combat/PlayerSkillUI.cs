using UnityEngine;
using ShanHai_IsolatedCity.Skill;
public class PlayerSkillUI : Singleton<PlayerSkillUI>
{
    public GameObject skillPanel;
    public GameObject skillHolder;
    public GameObject skillSlotPrefab;

    private Skills playerSkill;

    private void OnEnable()
    {
        EventHandler.UpdatePlayerStateEvent += OnUpdatePlayerStateEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdatePlayerStateEvent -= OnUpdatePlayerStateEvent;
    }

    private void Start()
    {
        playerSkill = GameManager.Instance.playerInformation.GetComponent<Skills>();
    }

    private void OnUpdatePlayerStateEvent(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.和平:
                skillPanel.SetActive(false);
                break;
            case PlayerState.战斗:
                skillPanel.SetActive(true);
                SetUpSkillPanel();
                break;
        }
    }
    
    private void SetUpSkillPanel()
    {
        if (skillHolder.transform.childCount > 0)
        {
            for (int index = 0; index < skillHolder.transform.childCount; index++)
            {
                Destroy(skillHolder.transform.GetChild(index).gameObject);
            }
        }

        foreach(var skill in playerSkill.skillList)
        {
            var Nskill = Instantiate(skillSlotPrefab, skillHolder.transform).GetComponent<PlayerSkillSlot>();
            Nskill.UpdateSlot(skill, playerSkill.skillList.IndexOf(skill));
        }
    }

}

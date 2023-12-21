using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    public Image healthImage;
    public Image qiImage;
    public Image moraleImage;
    public Image woundImage;

    public Text healthText;
    public Text qiText;
    public Text moraleText;
    public Text woundText;

    public CharacterEvent_SO healthEvent;
    public CharacterEvent_SO qiEvent;
    public CharacterEvent_SO woundEvent;
    public CharacterEvent_SO moraleEvent;

    private void OnEnable()
    {
        healthEvent.onEventIsCalled += healthChange;
        qiEvent.onEventIsCalled += qiChange;
        woundEvent.onEventIsCalled += woundChange;
        moraleEvent.onEventIsCalled += moraleChange;
    }

    private void OnDisable()
    {
        healthEvent.onEventIsCalled -= healthChange;
        qiEvent.onEventIsCalled -= qiChange;
        woundEvent.onEventIsCalled -= woundChange;
        moraleEvent.onEventIsCalled -= moraleChange;
    }

    private void healthChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxHealth <= 0)
        {
            characterInformation.checkExceedLimit();
            onHealthChange(0);
        }
            
        else
        {
            float healthPercent = (float)characterInformation.CurrentHealth / characterInformation.MaxHealth;
            onHealthChange(healthPercent);
        }
        
    }
    private void qiChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxQi <= 0)
        {
            characterInformation.checkExceedLimit();
            onQiChange(0);
        }

        else
        {
            float qiPercent = (float)characterInformation.CurrentQi / characterInformation.MaxQi;
            onQiChange(qiPercent);
        }
        
    }
    private void woundChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxWound <= 0)
        {
            characterInformation.checkExceedLimit();
            onWoundChange(0);
        }
        else
        {
            float woundPercent = (float)characterInformation.CurrentWound / characterInformation.MaxWound;
            onWoundChange(woundPercent);
        }

    }
    private void moraleChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxMorale <= 0)
        {
            characterInformation.checkExceedLimit();
            onMoraleChange(0);
        }
        else
        {
            float moralePercent = (float)characterInformation.CurrentMorale / characterInformation.MaxMorale;
            onMoraleChange(moralePercent);
        }

    }


    /// <summary>
    /// Receive the change on the health percentage
    /// </summary>
    /// <param name="percentage">Percentage:CurrentHealth/MaxHealth</param>
    public void onHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;
        healthText.text = GameManager.Instance.playerInformation.CurrentHealth.ToString() + " / " + GameManager.Instance.playerInformation.MaxHealth.ToString();
    }
    public void onQiChange(float percentage)
    {
        qiImage.fillAmount = percentage;
        qiText.text = GameManager.Instance.playerInformation.CurrentQi.ToString() + " / " + GameManager.Instance.playerInformation.MaxQi.ToString();
    }
    public void onWoundChange(float percentage)
    {
        woundImage.fillAmount = percentage;
        woundText.text = GameManager.Instance.playerInformation.CurrentWound.ToString() + " / " + GameManager.Instance.playerInformation.MaxWound.ToString();
    }
    public void onMoraleChange(float percentage)
    {
        moraleImage.fillAmount = percentage;
        moraleText.text = GameManager.Instance.playerInformation.CurrentMorale.ToString() + " / " + GameManager.Instance.playerInformation.MaxMorale.ToString();
    }

}

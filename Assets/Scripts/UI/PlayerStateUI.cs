using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    public Image healthImage;
    public Image qiImage;
    public Image vigorImage;
    public Image moraleImage;
    public Image woundImage;

    public Text healthText;
    public Text qiText;
    public Text vigorText;
    public Text moraleText;
    public Text woundText;

    public CharacterEvent_SO healthEvent;
    public CharacterEvent_SO qiEvent;
    public CharacterEvent_SO vigorEvent;
    public CharacterEvent_SO woundEvent;
    public CharacterEvent_SO moraleEvent;


    private void OnEnable()
    {
        healthEvent.OnEventIsCalled += HealthChange;
        qiEvent.OnEventIsCalled += QiChange;
        vigorEvent.OnEventIsCalled += VigorChange;
        woundEvent.OnEventIsCalled += WoundChange;
        moraleEvent.OnEventIsCalled += MoraleChange;
    }

    

    private void OnDisable()
    {
        healthEvent.OnEventIsCalled -= HealthChange;
        qiEvent.OnEventIsCalled -= QiChange;
        vigorEvent.OnEventIsCalled -= VigorChange;
        woundEvent.OnEventIsCalled -= WoundChange;
        moraleEvent.OnEventIsCalled -= MoraleChange;
    }

    private void HealthChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxHealth <= 0)
        {
            characterInformation.CheckExceedLimit();
            OnHealthChange(0);
        }
            
        else
        {
            float healthPercent = (float)characterInformation.CurrentHealth / characterInformation.MaxHealth;
            OnHealthChange(healthPercent);
        }
        
    }
    private void QiChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxQi <= 0)
        {
            characterInformation.CheckExceedLimit();
            OnQiChange(0);
        }

        else
        {
            float qiPercent = (float)characterInformation.CurrentQi / characterInformation.MaxQi;
            OnQiChange(qiPercent);
        }
        
    }
    private void VigorChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxVigor <= 0)
        {
            characterInformation.CheckExceedLimit();
            OnVigorChange(0);
        }
        else
        {
            float vigorPercent = (float)characterInformation.CurrentVigor / characterInformation.MaxVigor;
            OnVigorChange(vigorPercent);
        }

    }
    private void WoundChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxWound <= 0)
        {
            characterInformation.CheckExceedLimit();
            OnWoundChange(0);
        }
        else
        {
            float woundPercent = (float)characterInformation.CurrentWound / characterInformation.MaxWound;
            OnWoundChange(woundPercent);
        }

    }
    private void MoraleChange(CharacterInformation characterInformation)
    {
        if (characterInformation.MaxMorale <= 0)
        {
            characterInformation.CheckExceedLimit();
            OnMoraleChange(0);
        }
        else
        {
            float moralePercent = (float)characterInformation.CurrentMorale / characterInformation.MaxMorale;
            OnMoraleChange(moralePercent);
        }

    }


    /// <summary>
    /// Receive the change on the health percentage
    /// </summary>
    /// <param name="percentage">Percentage:CurrentHealth/MaxHealth</param>
    public void OnHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;
        healthText.text = GameManager.Instance.playerInformation.CurrentHealth.ToString() + " / " + GameManager.Instance.playerInformation.MaxHealth.ToString();
    }
    public void OnQiChange(float percentage)
    {
        qiImage.fillAmount = percentage;
        qiText.text = GameManager.Instance.playerInformation.CurrentQi.ToString() + " / " + GameManager.Instance.playerInformation.MaxQi.ToString();
    }
    private void OnVigorChange(float percentage)
    {
        vigorImage.fillAmount = percentage;
        vigorText.text = GameManager.Instance.playerInformation.CurrentVigor.ToString() + " / " + GameManager.Instance.playerInformation.MaxVigor.ToString();
    }
    public void OnWoundChange(float percentage)
    {
        woundImage.fillAmount = percentage;
        woundText.text = GameManager.Instance.playerInformation.CurrentWound.ToString() + " / " + GameManager.Instance.playerInformation.MaxWound.ToString();
    }
    public void OnMoraleChange(float percentage)
    {
        moraleImage.fillAmount = percentage;
        moraleText.text = GameManager.Instance.playerInformation.CurrentMorale.ToString() + " / " + GameManager.Instance.playerInformation.MaxMorale.ToString();
    }

}

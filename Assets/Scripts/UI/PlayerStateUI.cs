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

    private void Update()
    {
        updateHealth();
    }


    /// <summary>
    /// Receive the change on the health percentage
    /// </summary>
    /// <param name="percentage">Percentage:CurrentHealth/MaxHealth</param>
    public void onHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;
    }

    //FiXEDME:Using event to listen the data change rather than update it evenytime
    private void updateHealth()
    {
        float healthPercentage = (float)GameManager.Instance.playerInformation.CurrentHealth / GameManager.Instance.playerInformation.MaxHealth;
        onHealthChange(healthPercentage);
        healthText.text = GameManager.Instance.playerInformation.CurrentHealth.ToString() + " / " + GameManager.Instance.playerInformation.MaxHealth.ToString();
    }

}

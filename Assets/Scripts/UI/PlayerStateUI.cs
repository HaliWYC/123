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

    /// <summary>
    /// Receive the change on the health percentage
    /// </summary>
    /// <param name="percentage">Percentage:CurrentHealth/MaxHealth</param>
    public void onHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;
    }
}

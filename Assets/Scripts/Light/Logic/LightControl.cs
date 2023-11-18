using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class LightControl : MonoBehaviour
{
    public LightPatternList_SO lightPattern;

    private Light2D currentLight;
    private LightDetails currenLightDetails;

    private void Awake()
    {
        currentLight = GetComponent<Light2D>();
        
    }

    public void changeLightShift(Seasons seasons, LightShift lightShift, float timeDifference)
    {
        currenLightDetails = lightPattern.getLightDetails(seasons, lightShift);

        if (timeDifference / Settings.lightChangeDurationModification < Settings.lightChangeDuration)
        {
            var colorOffSet = (currenLightDetails.lightColor - currentLight.color) / Settings.lightChangeDuration * timeDifference;
            currentLight.color += colorOffSet;
            DOTween.To(() => currentLight.color, c => currentLight.color = c, currenLightDetails.lightColor, Settings.lightChangeDuration - timeDifference);
            DOTween.To(() => currentLight.intensity, i => currentLight.intensity = i, currenLightDetails.lightAmount, Settings.lightChangeDuration - timeDifference);
        }
        if (timeDifference / Settings.lightChangeDurationModification > Settings.lightChangeDuration )
        {
            currentLight.color = currenLightDetails.lightColor;
            currentLight.intensity = currenLightDetails.lightAmount;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : Singleton<LightManager>
{
    public LightControl[] sceneLights;
    private LightShift currentLightShift;
    private Seasons currentSeason;
    private float timeDifference=Settings.lightChangeDuration;
    private void OnEnable()
    {
        EventHandler.afterSceneLoadedEvent += onAfterSceneLoadedEvent;
        EventHandler.lightShiftEvent += onLightShiftEvent;
    }

    private void OnDisable()
    {
        EventHandler.afterSceneLoadedEvent -= onAfterSceneLoadedEvent;
        EventHandler.lightShiftEvent -= onLightShiftEvent;
    }

    private void onLightShiftEvent(Seasons seasons, LightShift lightShift, float timeDifference)
    {
        currentSeason = seasons;
        
        this.timeDifference = timeDifference;
        if (currentLightShift != lightShift)
        {
            currentLightShift = lightShift;
            
            foreach (LightControl light in sceneLights)
            {
                light.changeLightShift(currentSeason, currentLightShift, timeDifference);
            }
        }

    }

    private void onAfterSceneLoadedEvent()
    {
        sceneLights = FindObjectsOfType<LightControl>();
        
        foreach(LightControl light in sceneLights)
        {
            light.changeLightShift(currentSeason, currentLightShift, timeDifference);
        }
    }
}

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
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.LightShiftEvent += OnLightShiftEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.LightShiftEvent -= OnLightShiftEvent;
    }

    private void OnLightShiftEvent(Seasons seasons, LightShift lightShift, float timeDifference)
    {
        currentSeason = seasons;
        
        this.timeDifference = timeDifference;
        if (currentLightShift != lightShift)
        {
            currentLightShift = lightShift;
            
            foreach (LightControl light in sceneLights)
            {
                light.ChangeLightShift(currentSeason, currentLightShift, timeDifference);
            }
        }

    }

    private void OnAfterSceneLoadedEvent()
    {
        sceneLights = FindObjectsByType<LightControl>(FindObjectsSortMode.None);
        
        foreach(LightControl light in sceneLights)
        {
            light.ChangeLightShift(currentSeason, currentLightShift, timeDifference);
        }
    }
}

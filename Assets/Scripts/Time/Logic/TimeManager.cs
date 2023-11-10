using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : Singleton<TimeManager>
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Seasons gameSeason = Seasons.春;

    private int monthInSeason = 3;

    private bool gameClockPause;

    private float tikTime;

    private float timeDifference;

    private TimeSpan dawn=new TimeSpan(24,0,0);

    public TimeSpan gameTime => new TimeSpan(gameHour, gameMinute, gameSecond);

    protected override void Awake()
    {
        base.Awake();
        newGameTime();
    }

    private void OnEnable()
    {
        EventHandler.beforeSceneUnloadEvent += onBeforeSceneUnloadEvent;
        EventHandler.afterSceneLoadedEvent += onAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.beforeSceneUnloadEvent -= onBeforeSceneUnloadEvent;
        EventHandler.afterSceneLoadedEvent -= onAfterSceneLoadedEvent;
    }

    private void onBeforeSceneUnloadEvent()
    {
        gameClockPause = true;
    }

    private void onAfterSceneLoadedEvent()
    {
        gameClockPause = false;
    }

    

    private void Start()
    {
        EventHandler.callGameMinuteEvent(gameMinute, gameHour, gameDay, gameSeason);
        EventHandler.callGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        EventHandler.callLightShiftEvent(gameSeason, getCurrentLightShift(), timeDifference);
    }
    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;
            if (tikTime >= Settings.secondThrehold)
            {
                tikTime -= Settings.secondThrehold;
                updateGameTime();
            }
        }
        if (Input.GetKey(KeyCode.T))
        {
            for(int i = 0; i < 60; i++)
            {
                updateGameTime();
            }
        }
    }
    
    private void newGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 6;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 1000;
        gameSeason = Seasons.春;
    }

    public void updateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;
            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;
                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;
                    if (gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;
                        if (gameMonth > 12)
                        {
                            gameMonth = 1;
                        }
                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
                            if (seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }
                            gameSeason = (Seasons)seasonNumber;
                        }
                    }

                }
                EventHandler.callGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
                
            }
            EventHandler.callGameMinuteEvent(gameMinute, gameHour, gameDay, gameSeason);
            //Switch Light
            EventHandler.callLightShiftEvent(gameSeason, getCurrentLightShift(), timeDifference);
        }
    }

    /// <summary>
    /// Return Lightshift and calculate timediffence
    /// </summary>
    /// <returns></returns>
    private LightShift getCurrentLightShift()
    {
        if (gameTime > Settings.dawnTime && gameTime < Settings.morningTime)//Dawn to Morning
        {
            timeDifference = (float)(gameTime - Settings.dawnTime).TotalMinutes;
            
            return LightShift.破晓;
        }
        if (gameTime > Settings.morningTime && gameTime < Settings.eveningTime)//Moring to Evening
        {
            timeDifference = (float)(gameTime - Settings.morningTime).TotalMinutes;
            
            return LightShift.清晨;
        }
        if (gameTime > Settings.eveningTime && gameTime < Settings.nightTime)//Moring to Evening
        {
            timeDifference = (float)(gameTime - Settings.eveningTime).TotalMinutes;
            
            return LightShift.黄昏;
        }
        if (gameTime > Settings.nightTime && gameTime < dawn)//Moring to Evening
        {
            timeDifference =Mathf.Abs((float)(gameTime - Settings.nightTime).TotalMinutes);
            
            return LightShift.夜晚;
        }


        /*if (gameTime > Settings.morningTime && gameTime < Settings.nightTime)//Dawn to Morning
        {
            timeDifference = (float)(gameTime - Settings.morningTime).TotalMinutes;
            return LightShift.清晨;
        }
        if (gameTime > Settings.nightTime && gameTime < Settings.morningTime)//Moring to Evening
        {
            timeDifference = (float)(gameTime - Settings.nightTime).TotalMinutes;
            return LightShift.夜晚;
        }*/


        return LightShift.清晨;

    }
}

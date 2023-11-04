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
        gameHour = 7;
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
        }
    }
}

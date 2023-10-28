using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Seasons gameSeason = Seasons.春;

    private int monthInSeason = 3;

    private bool gameClockPause;

    private float tikTime;

    private void Awake()
    {
        newGameTime();
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
            }
        }
    }
}

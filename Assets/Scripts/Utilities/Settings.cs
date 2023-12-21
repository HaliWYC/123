using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public const float itemFadeDuration = 0.35f;

    public const float targetAlpha = 0.6f;

    //Time
    public const float secondThrehold = 0.1f; //Smaller value quicker time

    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;

    //Transition
    public const float fadeDuration = 1.5f;


    //NPC movement
    public const float gridCellSize = 1;
    public const float gridCellDiagonalSize = 1.41f;
    public const float pixelSize = 0.05f;
    public const float animationBreakTime = 5f;
    public const int maxGridSize = 9999;
    public const int stopDistance = 1;

    //Light
    public const float lightChangeDuration = 30f;
    public const float lightChangeDurationModification = 10;
    public static TimeSpan dawnTime = new TimeSpan(4, 0, 0);
    public static TimeSpan morningTime = new TimeSpan(7, 0, 0);
    public static TimeSpan eveningTime = new TimeSpan(18, 0, 0);
    public static TimeSpan nightTime = new TimeSpan(21, 0, 0);

    //Combat;
    public const float WoundRecoveryTime = 20;
    public const float penetrateConstant = 417;
    public const float criticalConstant = 213;
    public const float dodgeConstant = 171;
    public const float skillDodgeConstant = 213;
    //DamageTextPop
    public const float popDamageTextScaleSpeed = 1.0f;
    public const float popDamageTextDisappearTime = 1.5f;
    public const float popDamageTextAlphaSpeed = 4.0f;
    //Skill
    public const float sustainEffectUpdateTime = 3f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LightPatternList_SO",menuName ="Light/LightPattern")]
public class LightPatternList_SO : ScriptableObject
{
    public List<LightDetails> lightPatternList;


    /// <summary>
    /// Return Light Details accroding to the season and lightshift
    /// </summary>
    /// <param name="season">Season</param>
    /// <param name="lightShift">TimePeriod</param>
    /// <returns></returns>
    public LightDetails getLightDetails(Seasons season,LightShift lightShift)
    {
        return lightPatternList.Find(l => l.seasons == season && l.lightShift == lightShift);
    }
}

[System.Serializable]
public class LightDetails
{
    public Seasons seasons;
    public LightShift lightShift;
    public Color lightColor;
    public float lightAmount;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SchedualDataList_SO",menuName ="NPC Schedual/SchedualDataList")]
public class ScheduleDataList_SO : ScriptableObject 
{
    public List<ScheduleDetails> scheduleList;
}

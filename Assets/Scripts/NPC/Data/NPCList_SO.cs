using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewNPCList",menuName ="NPC/NPCList")]
public class NPCList_SO : ScriptableObject
{
    public List<NPCDetails> NPCDetailsList;
}

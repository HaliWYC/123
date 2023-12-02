using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="NEW CharacterEvent",menuName ="Character/CharacterEventSO")]
public class CharacterEvent_SO : ScriptableObject
{
    public UnityAction<CharacterInformation> onEventIsCalled;

    public void callEvent(CharacterInformation characterInformation)
    {
        onEventIsCalled?.Invoke(characterInformation);
    }
}

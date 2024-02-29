using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="NEW CharacterEvent",menuName ="Events/CharacterEventSO")]
public class CharacterEvent_SO : ScriptableObject
{
    public UnityAction<CharacterInformation> OnEventIsCalled;

    public void CallEvent(CharacterInformation characterInformation)
    {
        OnEventIsCalled?.Invoke(characterInformation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NEW VoidEvents", menuName = "Events/VoidEventSO")]
public class VoidEvent_SO : ScriptableObject
{
    public UnityAction OnEventIsCalled;

    public void CallEvent()
    {
        OnEventIsCalled?.Invoke();
    }
}

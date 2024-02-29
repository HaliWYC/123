using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    public VoidEvent_SO criticalShakeEvent;
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += SwitchConfinerShape;
        criticalShakeEvent.OnEventIsCalled += CriticalImpulseEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= SwitchConfinerShape;
        criticalShakeEvent.OnEventIsCalled -= CriticalImpulseEvent;
    }
    private void CriticalImpulseEvent()
    {
        impulseSource.GenerateImpulse();
    }
    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();
        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();
        cinemachineConfiner.m_BoundingShape2D = confinerShape;

        //call this if the bounding shape's points change at runtime
        cinemachineConfiner.InvalidatePathCache();
    }
}

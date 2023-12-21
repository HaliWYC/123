using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    public VoidEvent_SO criticalShakeEvent;
    private void OnEnable()
    {
        EventHandler.afterSceneLoadedEvent += SwitchConfinerShape;
        criticalShakeEvent.onEventIsCalled += criticalImpulseEvent;
    }

    private void OnDisable()
    {
        EventHandler.afterSceneLoadedEvent -= SwitchConfinerShape;
        criticalShakeEvent.onEventIsCalled -= criticalImpulseEvent;
    }
    private void criticalImpulseEvent()
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

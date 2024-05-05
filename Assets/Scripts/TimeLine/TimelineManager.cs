using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : Singleton<TimelineManager>
{
    public PlayableDirector StartCG;
    private PlayableDirector currentCG;

    protected override void Awake()
    {
        base.Awake();
        currentCG = StartCG;
    }

    
}

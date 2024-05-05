using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueClip : PlayableAsset,ITimelineClipAsset
{
    public ClipCaps clipCaps => ClipCaps.None;

    public DialogueBehavior dialogue = new DialogueBehavior();

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playables = ScriptPlayable<DialogueBehavior>.Create(graph, dialogue);
        return playables;
    }
}

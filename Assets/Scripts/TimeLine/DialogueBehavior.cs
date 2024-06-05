using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using ShanHai_IsolatedCity.Dialogue;

[System.Serializable]
public class DialogueBehavior : PlayableBehaviour
{
    private PlayableDirector director;
    public DialoguePiece dialoguePiece;
    public DialoguePieceOnlyText dialoguePieceOnlyText;

    public override void OnPlayableCreate(Playable playable)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (Application.isPlaying)
        {
            if (dialoguePiece.dialogueText != string.Empty)
            {
                EventHandler.CallUpdateDialoguePieceEvent(dialoguePiece);
                if (dialoguePiece.hasToPause)
                {
                    TimelineManager.Instance.PasueTime(director);
                }
                else
                {
                    EventHandler.CallUpdateDialoguePieceEvent(null);
                }

            }
           else if (dialoguePieceOnlyText.dialogueText != string.Empty)
            {
                EventHandler.CallShowDialoguePieceOnlyTextEvent(dialoguePieceOnlyText);
            }
            
        }
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (Application.isPlaying)
            TimelineManager.Instance.IsEnd = dialoguePiece.isEnd;
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        EventHandler.CallUpdateDialoguePieceEvent(null);
        EventHandler.CallShowDialoguePieceOnlyTextEvent(null);
    }

    public override void OnGraphStart(Playable playable)
    {
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }
    public override void OnGraphStop(Playable playable)
    {
        EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
    }
}

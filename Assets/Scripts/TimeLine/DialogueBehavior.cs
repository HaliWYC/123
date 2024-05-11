using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using ShanHai_IsolatedCity.Dialogue;

[System.Serializable]
public class DialogueBehavior : PlayableBehaviour
{
    private PlayableDirector director;
    public DialoguePieceWithBox dialoguePieceWithBox;
    public DialoguePiece dialoguePiece;

    public override void OnPlayableCreate(Playable playable)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (Application.isPlaying)
        {
            if (dialoguePieceWithBox.dialogueText != string.Empty)
            {
                EventHandler.CallShowDialogueEvent(dialoguePieceWithBox);
                if (dialoguePieceWithBox.hasToPause)
                {
                    TimelineManager.Instance.PasueTime(director);
                }
                else
                {
                    EventHandler.CallShowDialogueEvent(null);
                }

            }
           else if (dialoguePiece.dialogueText != string.Empty)
            {
                    EventHandler.CallShowDialoguePieceEvent(dialoguePiece);
            }
            
        }
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (Application.isPlaying)
            TimelineManager.Instance.IsEnd = dialoguePieceWithBox.isEnd;
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        EventHandler.CallShowDialogueEvent(null);
        EventHandler.CallShowDialoguePieceEvent(null);
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

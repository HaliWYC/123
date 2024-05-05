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
}

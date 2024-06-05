using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShanHai_IsolatedCity.Dialogue
{
    [System.Serializable]
    public class DialoguePiece
    {
        public string ID;
        public string targetID;

        [Header("对话详情")]
        public Sprite faceImage;
        public bool isLeft;
        public string name;

        [TextArea]
        public string dialogueText;
        public bool hasToPause;
        
        [HideInInspector]public bool isEnd;
        public bool finishTalk;
        public TaskData_SO task;

        [Header("Option")]
        public List<DialogueOption> dialogueOptions = new List<DialogueOption>();
    }

    [System.Serializable]
    public class DialoguePieceOnlyText
    {
        public string name;
        public DialoguePieceType dialogueType;

        [TextArea]
        public string dialogueText;
    }

    [System.Serializable]
    public class DialogueOption
    {
        public string TargetID;
        public string OptionName;
        public DialogueOptionType optionType;
        public DialoguePiece_SO dialogueData;
    }
}


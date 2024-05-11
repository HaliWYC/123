using UnityEngine;
using UnityEngine.Events;

namespace ShanHai_IsolatedCity.Dialogue
{
    [System.Serializable]
    public class DialoguePieceWithBox
    {
        [Header("对话详情")]
        public Sprite faceImage;
        public bool isLeft;
        public string name;

        [TextArea]
        public string dialogueText;
        public bool hasToPause;
        [HideInInspector]public bool isEnd;
        public UnityEvent onFinishEvent;
    }

    [System.Serializable]
    public class DialoguePiece
    {
        public string name;
        public DialoguePieceType dialogueType;

        [TextArea]
        public string dialogueText;
    }

}


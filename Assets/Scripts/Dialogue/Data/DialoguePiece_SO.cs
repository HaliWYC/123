using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShanHai_IsolatedCity.Dialogue
{
    [CreateAssetMenu(fileName = "NEW Talk", menuName = "Dialogue/DialogueData")]
    public class DialoguePiece_SO : ScriptableObject
    {
        public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();

        public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

        public List<DialoguePieceOnlyText> FinishDialogues = new List<DialoguePieceOnlyText>();

#if UNITY_EDITOR
        void OnValidate()
        {
            dialogueIndex.Clear();
            foreach (var piece in dialoguePieces)
            {
                if (!dialogueIndex.ContainsKey(piece.ID))
                    dialogueIndex.Add(piece.ID, piece);
            }
        }
#else
    void Awake()//保证在打包执行的游戏里第一时间获得对话的所有字典匹配 
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
                dialogueIndex.Add(piece.ID, piece);
        }
    }
#endif
        public DialoguePiece GetDialoguePiece(string ID)
        {
            return dialoguePieces.Find(i => i.ID == ID);
        }
    }
}

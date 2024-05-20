using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShanHai_IsolatedCity.Dialogue
{
    [RequireComponent(typeof(NPCMovement))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogueController : MonoBehaviour
    {
        private NPCMovement npc => GetComponent<NPCMovement>();
        private NPCDetails NPCDetails => NPCManager.Instance.GetNPCDetail(GetComponent<EnemyController>().NPCID);

        [Header("Dialogue Data")]
        public DialoguePiece_SO currentData;

        //public List<DialoguePieceWithBox> dialogueList = new List<DialoguePieceWithBox>();
        //public Stack<DialoguePieceWithBox> dialogueStack;

        private bool canInteract;
        private bool isTalking;

        private GameObject uiSign;

        private void Awake()
        {
            uiSign = transform.GetChild(1).gameObject;
        }

        private void OnEnable()
        {
            EventHandler.ShowDialoguePieceEvent += OnShowDialogueWithBoxEvent;
        }

        private void OnDisable()
        {
            EventHandler.ShowDialoguePieceEvent -= OnShowDialogueWithBoxEvent;
        }

        private void OnShowDialogueWithBoxEvent(DialoguePiece piece)
        {
            if (piece == null)
                isTalking = false;
        }

        private void Update()
        {
            uiSign.SetActive(canInteract);
            if (Input.GetKeyDown(KeyCode.Space) && canInteract)
            {
                if (!isTalking)
                {
                    isTalking = true;
                    EventHandler.CallUpdateGameStateEvent(GameState.Pause);
                    EventHandler.CallUpdateDialogueDataEvent(currentData);
                    EventHandler.CallShowDialoguePieceEvent(currentData.dialoguePieces[0]);
                }
                else if (DialogueUI.Instance.continueBox.activeInHierarchy)
                {
                    int index = DialogueUI.Instance.currentIndex;
                    if (currentData.dialoguePieces[index - 1].dialogueOptions.Count > 0)
                    {
                        DialogueUI.Instance.CreateOptions(currentData.dialoguePieces[index - 1]);
                    }
                    else
                    {
                        if (index < currentData.dialoguePieces.Count && !currentData.dialoguePieces[index - 1].finishTalk)
                        {
                            if (currentData.dialoguePieces[index - 1].targetID != string.Empty && currentData.dialogueIndex.ContainsKey(currentData.dialoguePieces[index - 1].targetID))
                            {
                                StartCoroutine(DialogueUI.Instance.ShowDialogue(currentData.dialogueIndex[currentData.dialoguePieces[index - 1].targetID]));
                            }
                            else
                            {
                                StartCoroutine(DialogueUI.Instance.ShowDialogue(currentData.dialoguePieces[index]));
                            }
                        }
                        else
                        {
                            EventHandler.CallShowDialoguePieceEvent(null);
                        }
                    }

                }
            }
                
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            canInteract = !npc.isNPCMoving && npc.interactable; 
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                canInteract = false;
        }


        //private void FillDialogueStack()
        //{
        //    dialogueStack = new Stack<DialoguePieceWithBox>();

        //    for(int i = dialogueList.Count - 1; i > -1; i--)
        //    {
        //        dialogueList[i].isEnd = false;
        //        dialogueStack.Push(dialogueList[i]);
        //    }
        //}

        //private IEnumerator DialogueRoutine()
        //{
        //    //TODO:Add only text dialoguepiece
        //    isTalking = true;
        //    if(dialogueStack.TryPop(out DialoguePieceWithBox result))
        //    {
        //        EventHandler.CallShowDialogueEvent(result);
        //        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
        //        yield return new WaitUntil(() => result.isEnd);
        //        isTalking = false;
        //    }
        //    else
        //    {
        //        EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
        //        EventHandler.CallShowDialogueEvent(null);
        //        FillDialogueStack();
                
        //        isTalking = false;

        //        if (onFinishEvent != null)
        //        {
        //            onFinishEvent.Invoke();
        //            canInteract = false;
        //        }
        //    }
        //}
    }

}

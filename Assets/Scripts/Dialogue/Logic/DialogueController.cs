using System.Collections;
using System.Collections.Generic;
using ShanHai_IsolatedCity.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace ShanHai_IsolatedCity.Dialogue
{
    [RequireComponent(typeof(NPCMovement))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogueController : MonoBehaviour
    {
        private NPCMovement npc => GetComponent<NPCMovement>();
        private NPCDetails NPCDetails ;

        [Header("Dialogue Data")]
        [SerializeField]private DialoguePiece_SO openingDialogue;
        public DialoguePiece_SO currentData;
        private DialogueOptionType optionFinishEvent;
        private bool haveFinishEvent = false;
        //public List<DialoguePieceWithBox> dialogueList = new List<DialoguePieceWithBox>();
        //public Stack<DialoguePieceWithBox> dialogueStack;

        private bool canInteract;
        private bool isTalking;
        private bool isAvailable;

        private GameObject uiSign;

        private void Awake()
        {
            uiSign = transform.GetChild(1).gameObject;

        }

        private void OnEnable()
        {
            EventHandler.UpdateDialoguePieceEvent += OnUpdateDialoguePieceEvent;
            EventHandler.UpdateDialogueDataEvent += OnUpdateDialogueDataEvent;
            EventHandler.UpdateDialogueOptionEvent += OnUpdateDialogueOptionEvent;
            EventHandler.NPCAvailableEvent += OnNPCAvailableEvent;
        }

        private void OnDisable()
        {
            EventHandler.UpdateDialoguePieceEvent -= OnUpdateDialoguePieceEvent;
            EventHandler.UpdateDialogueDataEvent -= OnUpdateDialogueDataEvent;
            EventHandler.UpdateDialogueOptionEvent -= OnUpdateDialogueOptionEvent;
            EventHandler.NPCAvailableEvent -= OnNPCAvailableEvent;
        }

        private void OnNPCAvailableEvent(bool available)
        {
            isAvailable = available;
            if(!isAvailable)
                EventHandler.CallUpdateGameStateEvent(GameState.Pause);
            else
                EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
        }

        private void OnUpdateDialogueOptionEvent(DialogueOptionType type)
        {
            optionFinishEvent = type;
            haveFinishEvent = true;
        }

        private void OnUpdateDialogueDataEvent(DialoguePiece_SO DP_SO)
        {
            currentData = DP_SO;
            haveFinishEvent = false;
        }

        private void OnUpdateDialoguePieceEvent(DialoguePiece piece)
        {
            if (piece == null)
                isTalking = false;
        }
        private void Start()
        {
            NPCDetails = NPCManager.Instance.GetNPCDetail(GetComponent<EnemyController>().NPCID);
            isAvailable = true;
        }
        private void Update()
        {
            uiSign.SetActive(canInteract && !isTalking);
            if (Input.GetKeyDown(KeyCode.Space) && canInteract && isAvailable)
            {
                if (!isTalking)
                {
                    isTalking = true;
                    EventHandler.CallUpdateGameStateEvent(GameState.Pause);
                    EventHandler.CallUpdateDialogueDataEvent(currentData);
                    EventHandler.CallUpdateDialoguePieceEvent(currentData.dialoguePieces[0]);
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
                                EventHandler.CallUpdateDialoguePieceEvent(currentData.dialogueIndex[currentData.dialoguePieces[index - 1].targetID]);
                            }
                            else
                            {
                                EventHandler.CallUpdateDialoguePieceEvent(currentData.dialoguePieces[index]);
                            }
                        }
                        else
                        {

                            EventHandler.CallUpdateDialoguePieceEvent(null);
                            if (haveFinishEvent)
                            {
                                CallFinishEvent();
                            }
                            else
                            {
                                EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
                            }
                            EventHandler.CallUpdateDialogueDataEvent(openingDialogue);

                        }
                    }

                }
            }   
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            canInteract = !npc.isNPCMoving && npc.interactable;
            currentData = openingDialogue;
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                canInteract = false;
        }

        private void CallFinishEvent()
        {
            switch (optionFinishEvent)
            {
                case DialogueOptionType.Task:
                    Task();
                    break;
                case DialogueOptionType.Trade:
                    Trade();
                    break;
                case DialogueOptionType.GivePresent:
                    GivePresent();
                    break;
                case DialogueOptionType.Leave:
                    Leave();
                    break;
            }
        }

        private void Task()
        {
            EventHandler.CallNPCAvailableEvent(false);
            if (currentData.GetTask()!= null)
            {
                var newTask = new TaskManager.Task
                {
                    taskData = Instantiate(currentData.GetTask())
                };

                if (TaskManager.Instance.HaveTask(newTask.taskData))
                {
                    if (TaskManager.Instance.GetTask(newTask.taskData).IsFinished)
                    {
                        newTask.taskData.GiveRewards();
                        TaskManager.Instance.GetTask(newTask.taskData).IsCompleted = true;
                    }
                }
                else
                {
                    TaskManager.Instance.tasks.Add(newTask);
                    TaskManager.Instance.GetTask(newTask.taskData).IsStarted = true;
                    foreach (var requireItem in newTask.taskData.GenerateRequirementsNameList())
                    {
                        InventoryManager.Instance.CheckItemInBag(requireItem);
                    }
                }
            }
            EventHandler.CallNPCAvailableEvent(true);
        }

        private void Trade()
        {
            EventHandler.CallNPCAvailableEvent(false);
            TradeUI.Instance.SetUpTraderBag(GetComponent<CharacterInformation>(), NPCDetails, InventoryManager.Instance.playerBag);
        }

        private void GivePresent()
        {
            EventHandler.CallNPCAvailableEvent(false);
            GiftUI.Instance.SetUpGiftUI(GetComponent<CharacterInformation>().characterInformation, NPCDetails);
        }

        private void Leave()
        {
            EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
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

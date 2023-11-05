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
        
        public List<DialoguePiece> dialogueList = new List<DialoguePiece>();

        public Stack<DialoguePiece> dialogueStack;

        public UnityEvent onFinishEvent;

        private bool canInteract;
        private bool isTalking;

        private GameObject uiSign;

        private void Awake()
        {
            fillDialogueStack();
            uiSign = transform.GetChild(1).gameObject;
        }

         
        private void Update()
        {
            uiSign.SetActive(canInteract);

            if(canInteract & Input.GetKeyDown(KeyCode.Space) && !isTalking)
            {
                StartCoroutine(dialogueRoutine()); 
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            canInteract = !npc.isMoving && npc.interactable; 
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                canInteract = false;
        }


        private void fillDialogueStack()
        {
            dialogueStack = new Stack<DialoguePiece>();

            for(int i = dialogueList.Count - 1; i > -1; i--)
            {
                dialogueList[i].isEnd = false;
                dialogueStack.Push(dialogueList[i]);
            }
        }

        private IEnumerator dialogueRoutine()
        {
            isTalking = true;
            if(dialogueStack.TryPop(out DialoguePiece result))
            {
                EventHandler.callShowDialogueEvent(result);
                EventHandler.callUpdateGameStateEvent(GameState.Pause);
                yield return new WaitUntil(() => result.isEnd);
                isTalking = false;
            }
            else
            {
                EventHandler.callUpdateGameStateEvent(GameState.GamePlay);
                EventHandler.callShowDialogueEvent(null);
                fillDialogueStack();
                
                isTalking = false;

                if (onFinishEvent != null)
                {
                    onFinishEvent.Invoke();
                    canInteract = false;
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShanHai_IsolatedCity.Dialogue;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text dialogueText;
    public Image faceLeft, faceRight;
    public TextMeshProUGUI nameLeft, appellationLeft, nameRight, appellaionRight;
    public GameObject continueBox;


    private void Awake()
    {
        continueBox.SetActive(false);
    }

    private void OnEnable()
    {
        EventHandler.showDialogueEvent += onShowDialogueEvent;
    }

    private void OnDisable()
    {
        EventHandler.showDialogueEvent -= onShowDialogueEvent;
    }

    private void onShowDialogueEvent(DialoguePiece dialoguePiece)
    {
        StartCoroutine(showDialogue(dialoguePiece));
    }

    private IEnumerator showDialogue(DialoguePiece dialoguePiece)
    {
        if (dialoguePiece != null)
        {
            dialoguePiece.isEnd = false;

            dialogueBox.SetActive(true);
            continueBox.SetActive(false);

            dialogueText.text = string.Empty;
            //TODO:Remember to add the appellation for the character

            if (dialoguePiece.name != string.Empty)
            {
                if (dialoguePiece.isLeft)
                {
                    faceRight.gameObject.SetActive(false);
                    faceLeft.gameObject.SetActive(true);
                    faceLeft.sprite = dialoguePiece.faceImage;
                    nameLeft.text = dialoguePiece.name;
                }
                else
                {
                    faceRight.gameObject.SetActive(true);
                    faceLeft.gameObject.SetActive(false);
                    faceRight.sprite = dialoguePiece.faceImage;
                    nameRight.text = dialoguePiece.name;
                }
            }
            else
            {
                faceRight.gameObject.SetActive(false);
                faceLeft.gameObject.SetActive(false);
                nameLeft.gameObject.SetActive(false);
                nameRight.gameObject.SetActive(false);
            }
            yield return dialogueText.DOText(dialoguePiece.dialogueText, 1f).WaitForCompletion();

            dialoguePiece.isEnd = true;

            if (dialoguePiece.hasToPause && dialoguePiece.isEnd)
            {
                continueBox.SetActive(true);
            }
            
        }
        else
        {
            dialogueBox.SetActive(false);
            
            yield break;
        }
        
    }
}

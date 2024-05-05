using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShanHai_IsolatedCity.Dialogue;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject dialogueBox;
    public GameObject dialoguePiecePrefab;

    [Header("DialoguePieceWithBox")]
    public Text dialogueWithBoxText;
    public Image faceLeft, faceRight;
    public TextMeshProUGUI nameLeft, appellationLeft, nameRight, appellaionRight;
    public GameObject continueBox;

    private void Awake()
    {
        continueBox.SetActive(false);
    }

    private void OnEnable()
    {
        EventHandler.ShowDialogueWithBoxEvent += OnShowDialogueWithBoxEvent;
        EventHandler.ShowDialoguePieceEvent += OnShowDialoguePieceEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowDialogueWithBoxEvent -= OnShowDialogueWithBoxEvent;
        EventHandler.ShowDialoguePieceEvent -= OnShowDialoguePieceEvent;
    }

    private void OnShowDialoguePieceEvent(DialoguePiece dialoguePiece)
    {
        if (dialoguePiece != null)
        {
            dialogueBox.SetActive(true);
            Text piece = Instantiate(dialoguePiecePrefab, dialogueBox.transform).GetComponent<Text>();
            if (dialoguePiece.name != string.Empty)
                piece.DOText((dialoguePiece.name + ": " + dialoguePiece.dialogueText), 2f);
            else
                piece.DOText(dialoguePiece.dialogueText, 2f);
            Destroy(piece.gameObject, 5f);
        }
        else
            dialogueBox.SetActive(false);
        
        
    }

    private void OnShowDialogueWithBoxEvent(DialoguePieceWithBox dialoguePiece)
    {
        StartCoroutine(ShowDialogue(dialoguePiece));
    }

    private IEnumerator ShowDialogue(DialoguePieceWithBox dialoguePiece)
    {
        if (dialoguePiece != null)
        {
            dialoguePiece.isEnd = false;
            dialoguePanel.SetActive(true);
            continueBox.SetActive(false);
            dialogueWithBoxText.text = string.Empty;
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
            yield return dialogueWithBoxText.DOText(dialoguePiece.dialogueText, 1f).WaitForCompletion();

            dialoguePiece.isEnd = true;

            if (dialoguePiece.hasToPause && dialoguePiece.isEnd)
            {
                continueBox.SetActive(true);
            }
        }
        else
        {
            dialoguePanel.SetActive(false);
            yield break;
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShanHai_IsolatedCity.Dialogue;
using DG.Tweening;

public class DialogueUI : Singleton<DialogueUI>
{
    public GameObject dialoguePanel;
    public GameObject dialoguePanelOnlyText;
    public GameObject dialoguePiecePrefab;

    [Header("DialoguePiece")]
    public Text dialoguePieceText;
    public Image faceLeft, faceRight;
    public TextMeshProUGUI nameLeft, appellationLeft, nameRight, appellaionRight;
    public GameObject continueBox;

    [Header("DialogueData")]
    public DialoguePiece_SO currentData;
    public int currentIndex = 0;

    [Header("Option Panel")]
    public RectTransform optionPanel;
    public GameObject optionPrefab;

    protected override void Awake()
    {
        base.Awake();
        continueBox.SetActive(false);
    }

    private void OnEnable()
    {
        EventHandler.UpdateDialoguePieceEvent += OnUpdateDialoguePieceEvent;
        EventHandler.ShowDialoguePieceOnlyTextEvent += OnShowDialoguePieceEvent;
        EventHandler.UpdateDialogueDataEvent += OnUpdateDialogueDataEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateDialoguePieceEvent -= OnUpdateDialoguePieceEvent;
        EventHandler.ShowDialoguePieceOnlyTextEvent -= OnShowDialoguePieceEvent;
        EventHandler.UpdateDialogueDataEvent -= OnUpdateDialogueDataEvent;
    }

    private void OnUpdateDialogueDataEvent(DialoguePiece_SO DP_SO)
    {
        currentData = DP_SO;
        currentIndex = 0;
    }

    private void OnShowDialoguePieceEvent(DialoguePieceOnlyText dialoguePiece)
    {
        if (dialoguePiece != null)
        {
            if(dialoguePiece.dialogueType== DialoguePieceType.OnlyText)
            {
                dialoguePanelOnlyText.SetActive(true);
                Text piece = Instantiate(dialoguePiecePrefab, dialoguePanelOnlyText.transform).GetComponent<Text>();
                piece.text = string.Empty;
                if (dialoguePiece.name != string.Empty)
                    piece.DOText((dialoguePiece.name + ": " + dialoguePiece.dialogueText), 1f);
                else
                    piece.DOText(dialoguePiece.dialogueText, 1f);
                Destroy(piece.gameObject, 3f);
            }
        }
        else
            dialoguePanelOnlyText.SetActive(false);
        
        
    }

    private void OnUpdateDialoguePieceEvent(DialoguePiece dialoguePiece)
    {
        StartCoroutine(ShowDialogue(dialoguePiece));
    }

    private IEnumerator ShowDialogue(DialoguePiece dialoguePiece)
    {
        if(currentData!=null)
            currentIndex = currentData.dialoguePieces.IndexOf(dialoguePiece) + 1;
        if (dialoguePiece != null)
        {

            dialoguePiece.isEnd = false;
            dialoguePanel.SetActive(true);
            continueBox.SetActive(false);
            dialoguePieceText.text = string.Empty;
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
                faceRight.enabled = false;
                faceLeft.enabled = false;
                nameLeft.enabled = false;
                nameRight.enabled = false;
            }
            yield return dialoguePieceText.DOText(dialoguePiece.dialogueText, 1f).WaitForCompletion();

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

    public void CreateOptions(DialoguePiece dialoguePiece)
    {
        dialoguePanel.SetActive(false);
        optionPanel.gameObject.SetActive(true);
        CleanOptions();
        for (int index = 0; index < dialoguePiece.dialogueOptions.Count; index++)
        {
            var option = Instantiate(optionPrefab, optionPanel.transform).GetComponent<OptionUI>();
            option.SetUpOptionUI(dialoguePiece, dialoguePiece.dialogueOptions[index]);
        }
    }
    public void CleanOptions()
    {
        if (optionPanel.childCount > 0)
        {
            for (int index = 0; index < optionPanel.childCount; index++)
            {
                Destroy(optionPanel.GetChild(index).gameObject);
            }
        }
    }

    public IEnumerator FinishDialogues()
    {
        EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
        if (currentData.FinishDialogues.Count > 0)
        {
            foreach (DialoguePieceOnlyText piece in currentData.FinishDialogues)
            {
                EventHandler.CallShowDialoguePieceOnlyTextEvent(piece);
                yield return new WaitForSeconds(2f);
            }
        }

    }
}

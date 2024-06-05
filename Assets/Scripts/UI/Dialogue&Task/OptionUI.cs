using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShanHai_IsolatedCity.Inventory;

namespace ShanHai_IsolatedCity.Dialogue
{
    public class OptionUI : MonoBehaviour
    {
        public TextMeshProUGUI OptionText;
        private Button button;

        private DialoguePiece currentPiece;
        private DialogueOption currentOption;
        private DialoguePiece_SO currentDialogue;
        private string nextPieceID;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnOptionClicked);
        }

        public void SetUpOptionUI(DialoguePiece dialoguePiece,DialogueOption option)
        {
            currentOption = option;
            currentDialogue = currentOption.dialogueData;
            OptionText.text = currentOption.OptionName;
            nextPieceID = currentOption.TargetID;
        }

        private void OnOptionClicked()
        {
            DialogueUI.Instance.CleanOptions();
            EventHandler.CallUpdateDialogueDataEvent(null);
            EventHandler.CallUpdateDialogueOptionEvent(currentOption.optionType);
            if (nextPieceID == string.Empty)
            {
                if(currentDialogue != null)
                {
                    EventHandler.CallUpdateTaskDataEvent(currentDialogue.GetTask());
                    if (!TaskManager.Instance.HaveTask(currentDialogue.GetTask()))
                    {
                        EventHandler.CallUpdateDialogueDataEvent(currentDialogue);
                        EventHandler.CallUpdateDialoguePieceEvent(currentDialogue.dialoguePieces[0]);
                    }
                }
                else
                {
                    EventHandler.CallUpdateDialoguePieceEvent(null);
                }
                //FIXME:Create plot finishdialogues when finish some plot option
                //StartCoroutine(DialogueUI.Instance.FinishDialogues());
            }
            else
            {
                if(DialogueUI.Instance.currentData.dialogueIndex.ContainsKey(nextPieceID))
                    EventHandler.CallUpdateDialoguePieceEvent(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
            }
        }

    }
}


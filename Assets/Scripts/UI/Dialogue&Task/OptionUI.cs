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
        private string nextPieceID;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnOptionClicked);
        }

        public void SetUpOptionUI(DialoguePiece dialoguePiece,DialogueOption option)
        {
            currentPiece = dialoguePiece;
            currentOption = option;
            OptionText.text = currentOption.Text;
            nextPieceID = currentOption.TargetID;
        }

        private void OnOptionClicked()
        {
            if (currentOption.task != null)
            {
                var newTask = new TaskManager.Task
                {
                    taskData = Instantiate(currentOption.task)
                };
                if (currentOption.TakeTask)
                {
                    if (TaskManager.Instance.HaveTask(newTask.taskData))
                    {

                    }
                    else
                    {
                        TaskManager.Instance.tasks.Add(newTask);
                        TaskManager.Instance.GetTask(newTask.taskData).IsStarted = true;

                        foreach(var requireItem in newTask.taskData.GenerateRequirementsNameList())
                        {
                            InventoryManager.Instance.CheckItemInBag(requireItem);
                        }
                    }
                }
                else
                {

                }
            }

            if (nextPieceID == string.Empty)
            {
                EventHandler.CallShowDialoguePieceEvent(null);
                //TODO:Switch here for different function
                //FIXME:Create plot finishdialogues when finish some plot option
                StartCoroutine(DialogueUI.Instance.FinishDialogues());
                return;
            }
            else
            {
                DialogueUI.Instance.CleanOptions();
                EventHandler.CallShowDialoguePieceEvent(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
            }
            //Destroy(gameObject);
        }
    }
}


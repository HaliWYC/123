using DG.Tweening;
using ShanHai_IsolatedCity.Dialogue;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelineManager : Singleton<TimelineManager>
{
    public PlayableDirector StartCG;
    private PlayableDirector currentCG;
    public GameObject dialoguePanelInCG;
    public Text dialogueTextinCG;

    private bool isPause;
    private bool isEnd;
    public bool IsEnd { set => isEnd = value; }

    protected override void Awake()
    {
        base.Awake();
        currentCG = StartCG;
    }

    private void OnEnable()
    {
        EventHandler.ShowDialoguePieceEvent += OnShowDialoguePieceEvent;
        currentCG.played += TimeLinePlay;
        currentCG.stopped += TimeLineStopped;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowDialoguePieceEvent -= OnShowDialoguePieceEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        currentCG = FindObjectOfType<PlayableDirector>();
        if (currentCG != null)
            currentCG.Play();
    }

    private void TimeLinePlay(PlayableDirector director)
    {
        if (director != null)
            EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }
    private void TimeLineStopped(PlayableDirector director)
    {
        if (director != null)
        {
            EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
            director.gameObject.SetActive(false);
        }

    }

    

    private void OnShowDialoguePieceEvent(DialoguePiece dialoguePiece)
    {
        if (dialoguePiece != null && dialoguePiece.dialogueType == DialoguePieceType.InCG)
        {
            dialogueTextinCG.text = string.Empty;
            dialoguePanelInCG.SetActive(true);
            dialogueTextinCG.DOText(dialoguePiece.dialogueText, 1f);
        }
        else
            dialoguePanelInCG.SetActive(false);
    }

    private void Update()
    {
        if(isPause && Input.GetKeyDown(KeyCode.Space) && isEnd)
        {
            isPause = false;
            currentCG.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        }
    }


    public void PasueTime(PlayableDirector director)
    {
        currentCG = director;
        currentCG.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        isPause = true;
    }
}

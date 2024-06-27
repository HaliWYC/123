using UnityEngine;


public class FunctionMenu : Singleton<FunctionMenu>
{
    public GameObject functionMenu;
    public GameObject timelineRecord;
    private bool isAvailable;

    private void OnEnable()
    {
        EventHandler.UpdateGameStateEvent += OnUpdateGameStateEvent;
    }
    private void OnDisable()
    {
        EventHandler.UpdateGameStateEvent -= OnUpdateGameStateEvent;
    }

    private void OnUpdateGameStateEvent(GameState state)
    {
        if (state == GameState.GamePlay)
            isAvailable = true;
        else
            isAvailable = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && isAvailable)
        {
            functionMenu.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (timelineRecord.activeInHierarchy)
            {
                functionMenu.GetComponentInChildren<FunctionMenuDirector>().director.Play();
                functionMenu.GetComponentInChildren<FunctionMenuDirector>().director.Pause();
                timelineRecord.SetActive(false);
            }
            else if (!timelineRecord.activeInHierarchy)
                functionMenu.SetActive(false);
        }

    }
}

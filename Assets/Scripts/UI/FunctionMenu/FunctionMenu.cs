using UnityEngine;


public class FunctionMenu : Singleton<FunctionMenu>
{
    public GameObject functionMenu;
    public GameObject timelineRecord;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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

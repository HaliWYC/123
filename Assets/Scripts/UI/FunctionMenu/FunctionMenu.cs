using UnityEngine;


public class FunctionMenu : MonoBehaviour
{
    public GameObject functionMenu;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            functionMenu.SetActive(true);
            functionMenu.GetComponentInChildren<FunctionMenuDirector>().playTimes = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            functionMenu.SetActive(false);
            functionMenu.GetComponentInChildren<FunctionMenuDirector>().playTimes = true;
        }

    }
}

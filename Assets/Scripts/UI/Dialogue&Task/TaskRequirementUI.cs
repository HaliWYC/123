using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskRequirementUI : MonoBehaviour
{
    private Text requireName;
    private Text progressNumber;

    private void Awake()
    {
        requireName = GetComponent<Text>();
        progressNumber = transform.GetChild(0).GetComponent<Text>();
    }


    public void SetUpTaskRequirement(string Name, int requireAmount, int currentAmount)
    {
        requireName.text = Name;
        progressNumber.text = currentAmount.ToString() + " / " + requireAmount.ToString();
    }
}

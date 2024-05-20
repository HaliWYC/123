using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class FunctionMenuDirector : MonoBehaviour
{
    public Button charButton;
    public Button abilityButton;
    public Button skillButton;
    public Button inventoryButton;
    public Button taskButton;
    public Button mapButton;
    public Button settingsButton;
    public PlayableDirector director;

    private void Awake()
    {
        charButton.onClick.AddListener(CharButtonEvent);
        abilityButton.onClick.AddListener(AbilityButtonEvent);
        skillButton.onClick.AddListener(SkillButtonEvent);
        inventoryButton.onClick.AddListener(InventoryButtonEvent);
        taskButton.onClick.AddListener(TaskButtonEvent);
        mapButton.onClick.AddListener(MapButtonEvent);
        settingsButton.onClick.AddListener(SettingsButtonEvent);
    }

    private void CharButtonEvent()
    {
        if (!FunctionMenu.Instance.timelineRecord.activeInHierarchy)
        {
            director.Play();
            FunctionMenu.Instance.timelineRecord.SetActive(true);
        }
        charButton.GetComponentInChildren<Toggle>().isOn = true;
    }
    private void AbilityButtonEvent()
    {
        if (!FunctionMenu.Instance.timelineRecord.activeInHierarchy)
        {
            director.Play();
            FunctionMenu.Instance.timelineRecord.SetActive(true);
        }

        abilityButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void SkillButtonEvent()
    {
        if (!FunctionMenu.Instance.timelineRecord.activeInHierarchy)
        {
            director.Play();
            FunctionMenu.Instance.timelineRecord.SetActive(true);
        }

        skillButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void InventoryButtonEvent()
    {
        if (!FunctionMenu.Instance.timelineRecord.activeInHierarchy)
        {
            director.Play();
            FunctionMenu.Instance.timelineRecord.SetActive(true);
        }

        inventoryButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void TaskButtonEvent()
    {
        if (!FunctionMenu.Instance.timelineRecord.activeInHierarchy)
        {
            director.Play();
            FunctionMenu.Instance.timelineRecord.SetActive(true);
        }

        taskButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void MapButtonEvent()
    {
        if (!FunctionMenu.Instance.timelineRecord.activeInHierarchy)
        {
            director.Play();
            FunctionMenu.Instance.timelineRecord.SetActive(true);
        }

        mapButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void SettingsButtonEvent()
    {
        if (!FunctionMenu.Instance.timelineRecord.activeInHierarchy)
        {
            director.Play();
            FunctionMenu.Instance.timelineRecord.SetActive(true);
        }

        settingsButton.GetComponentInChildren<Toggle>().isOn = true;
    }

    public void CallUpdateCharacterInformationUI()
    {
        EventHandler.CallUpdateCharacterInformationUIEvent();
    }
}

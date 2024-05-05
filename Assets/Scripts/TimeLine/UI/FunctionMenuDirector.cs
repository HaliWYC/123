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

    public bool playTimes;

    private void Awake()
    {
        playTimes = false;
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
        if (!playTimes)
        {
            director.Play();
        }
        playTimes = true;
        
        charButton.GetComponentInChildren<Toggle>().isOn = true;
    }
    private void AbilityButtonEvent()
    {
        if (!playTimes)
        {
            director.Play();
        }
        playTimes = true;

        abilityButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void SkillButtonEvent()
    {
        if (!playTimes)
        {
            director.Play();
        }
        playTimes = true;

        skillButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void InventoryButtonEvent()
    {
        if (!playTimes)
        {
            director.Play();
        }
        playTimes = true;

        inventoryButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void TaskButtonEvent()
    {
        if (!playTimes)
        {
            director.Play();
        }
        playTimes = true;

        taskButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void MapButtonEvent()
    {
        if (!playTimes)
        {
            director.Play();
        }
        playTimes = true;

        mapButton.GetComponentInChildren<Toggle>().isOn = true;

    }
    private void SettingsButtonEvent()
    {
        if (!playTimes)
        {
            director.Play();
        }
        playTimes = true;

        settingsButton.GetComponentInChildren<Toggle>().isOn = true;
    }

    public void CallUpdateCharacterInformationUI()
    {
        EventHandler.CallUpdateCharacterInformationUIEvent();
    }
}

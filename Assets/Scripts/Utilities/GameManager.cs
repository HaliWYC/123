using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterInformation playerInformation;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    public void registerPlayer(CharacterInformation player)
    {
        playerInformation = player;
    }

    public void addObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void removeObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void notifyObservers()
    {
        foreach(var observer in endGameObservers)
        {
            observer.endNotify();
        }
    }
}

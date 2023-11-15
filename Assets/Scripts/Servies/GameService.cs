using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using System;

public class GameService : IService
{
    public EventHandler<int> levelCompleteEvent;
    public int currentLevel;
    public const int maxLevel = 6;
    public GameService() { }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void NotifyLevelComplete()
    {
        if(currentLevel < maxLevel)
        {
            currentLevel++;
            levelCompleteEvent?.Invoke(this, currentLevel);
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        currentLevel = 0;
    }

    public void ShutDown()
    {
        throw new System.NotImplementedException();
    }
}

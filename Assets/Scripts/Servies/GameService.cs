using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using System;

public class GameService : IService
{
    int currentLevel;
    const int maxLevel = 6;
    public GameService() { }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void NotifyLevelComplete()
    {
        if(currentLevel < maxLevel -1)
        {
            currentLevel++;
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
}

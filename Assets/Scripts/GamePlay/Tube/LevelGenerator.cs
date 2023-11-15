using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using System;

public class LevelGenerator : MonoBehaviour
{
    GameObject currentTube;
    BallGenerator _ballGenerator;

    public void Initialize()
    {
        _ballGenerator = GetComponent<BallGenerator>();
    }

    public void LoadLevel(Level levelData)
    {
        if(!ReferenceEquals(currentTube, null))
        {
            Destroy(currentTube);
        }
        currentTube = Instantiate(levelData.tubePrefab, transform);
        _ballGenerator.GenerateLevel(levelData);
    }
}

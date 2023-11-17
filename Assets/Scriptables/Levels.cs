using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Tube Scriptable", menuName = "LevelData")]
public class Levels : ScriptableObject
{
    public Level[] levels;
}

[System.Serializable]
public class Level
{
    public GameObject tubePrefab;
    public Vector3 spawnOffset;
    public Rigidbody ballPrefab;
    public float spawnRadius = 1f;
    public float ballOffset = 1f;
    public int ballCount = 15;
    public int levelSuccessScore = 20;
    public TextAsset svgLevelFile;
}

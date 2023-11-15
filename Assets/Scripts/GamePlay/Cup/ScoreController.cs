using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;
    
    public void UpdateScore(string score)
    {
        scoreTxt.text = score;
    }
}

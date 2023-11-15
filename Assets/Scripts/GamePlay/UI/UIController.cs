using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject failedStatePanel;
    public GameObject successStatePanel;
    public Button failedActionButton;
    public Button successActionButton;

    public void Initialize(Action loadLevelOnClick)
    {
        failedActionButton.onClick.AddListener(() => { loadLevelOnClick?.Invoke();});
        successActionButton.onClick.AddListener(() => { loadLevelOnClick?.Invoke(); });
        HideStateUI();
    }

    public void ShowStateUI(bool isSuccess)
    {
        if(isSuccess)
        {
            successStatePanel.SetActive(true);
            return;
        }
        failedStatePanel.SetActive(true);
    }

    public void HideStateUI()
    {
        failedStatePanel.SetActive(false);
        successStatePanel.SetActive(false);
    }
    
}

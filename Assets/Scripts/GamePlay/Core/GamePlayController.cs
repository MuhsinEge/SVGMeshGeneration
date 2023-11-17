using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using System;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] CupCollector cupCollector;
    [SerializeField] GroundCollector groundCollector;
    [SerializeField] ScoreController scoreController;
    [SerializeField] UIController uiController;
    [SerializeField] LevelGenerator levelGenerator;
    [SerializeField] ParticleController particleController;

    public Levels levelDatas;

    private GameService _gameService;
    private int _collectedBallCount;
    private int _ballCountOnGround;
    private Level _currentLevelData;

    
    bool isLevelFinished;

    private void Awake()
    {
        _gameService = Locator.Current.Get<GameService>();
        cupCollector.ballCollectedEvent += OnBallCollected;
        groundCollector.ballFellToGroundEvent += OnBallFellToGround;
        levelGenerator.Initialize();
        uiController.Initialize(() => LoadLevel());
        LoadLevel();
    }

    private void OnBallCollected(object sender, Collider ballCollider)
    {
        if(!isLevelFinished) 
        {
            particleController.PlayCollectBallParticle();
        }
        _collectedBallCount++;
        CheckGameState();
        InactivateBall(ballCollider);
        
    }
    private void OnBallFellToGround(object sender, Collider ballCollider)
    {
        if (!isLevelFinished)
        {
            particleController.PlayBallOnGroundParticle();
        }
        _ballCountOnGround++;
        CheckGameState();
        InactivateBall(ballCollider);
    }

    private void InactivateBall(Collider ballCollider)
    {
        var rigidBody = ballCollider.GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.None;
        ballCollider.gameObject.tag = "InactiveBall";
    }

    private void CheckGameState()
    {
        var scoreTxt = _collectedBallCount + "/" + _currentLevelData.levelSuccessScore;
        scoreController.UpdateScore(scoreTxt);
        if(isLevelFinished)
        {
            return;
        }
        if (_collectedBallCount >= _currentLevelData.levelSuccessScore)
        {
            uiController.ShowStateUI(true);
            _gameService.NotifyLevelComplete();
            particleController.PlaySuccessParticle();
            isLevelFinished = true;
        }
        else
        {
            if (_ballCountOnGround + _collectedBallCount == _currentLevelData.ballCount)
            {
                isLevelFinished = true;
                uiController.ShowStateUI(false);
            }
        }
    }

    private void LoadLevel()
    {
        var currentLevel = _gameService.GetCurrentLevel();
        uiController.SetLevelText(currentLevel+1);
        _currentLevelData = levelDatas.levels[currentLevel];
        uiController.HideStateUI();
        _ballCountOnGround = 0;
        _collectedBallCount = 0;
        levelGenerator.LoadLevel(_currentLevelData);
        isLevelFinished = false;
        CheckGameState();
    }
}

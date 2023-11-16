using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using System;

public class GamePlayController : MonoBehaviour
{
    public CupCollector cupCollector;
    public GroundCollector groundCollector;
    public ScoreController scoreController;
    public UIController uiController;

    public LevelGenerator levelGenerator;
    public Levels levelDatas;

    private GameService _gameService;
    private int _collectedBallCount;
    private int _ballCountOnGround;
    private Level _currentLevelData;

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
        _collectedBallCount++;
        CheckGameState();
        InactivateBall(ballCollider);
        ballCollider.gameObject.tag = "CollectedBall";
    }
    private void OnBallFellToGround(object sender, Collider ballCollider)
    {
        _ballCountOnGround++;
        CheckGameState();
        InactivateBall(ballCollider);
        ballCollider.gameObject.tag = "BallOnGround";
    }

    private void InactivateBall(Collider ballCollider)
    {
        var rigidBody = ballCollider.GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.None;
    }

    private void CheckGameState()
    {
        var scoreTxt = _collectedBallCount + "/" + _currentLevelData.levelSuccessScore;
        scoreController.UpdateScore(scoreTxt);

        if (_ballCountOnGround + _collectedBallCount == _currentLevelData.ballCount)
        {
            if (_collectedBallCount >= _currentLevelData.levelSuccessScore)
            {
                uiController.ShowStateUI(true);
                _gameService.NotifyLevelComplete();
                Debug.Log("LEVEL COMPLETE");
            }
            else
            {
                uiController.ShowStateUI(false);
                Debug.Log("LEVEL FAILED");
            }
        }


    }

    private void LoadLevel()
    {
        _currentLevelData = levelDatas.levels[_gameService.GetCurrentLevel()];
        uiController.HideStateUI();
        _ballCountOnGround = 0;
        _collectedBallCount = 0;
        levelGenerator.LoadLevel(_currentLevelData);
        CheckGameState();
    }
}

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

    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem collectBallParticle;
    [SerializeField] ParticleSystem ballOnGroundParticle;
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
            collectBallParticle.Play();
        }
        _collectedBallCount++;
        CheckGameState();
        InactivateBall(ballCollider);
        ballCollider.gameObject.tag = "CollectedBall";
    }
    private void OnBallFellToGround(object sender, Collider ballCollider)
    {
        if (!isLevelFinished)
        {
            ballOnGroundParticle.Play();
        }
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
        if(isLevelFinished)
        {
            return;
        }
        if (_collectedBallCount >= _currentLevelData.levelSuccessScore)
        {
            uiController.ShowStateUI(true);
            _gameService.NotifyLevelComplete();
            successParticle.Play();
            isLevelFinished = true;
            Debug.Log("LEVEL COMPLETE");
        }
        else
        {
            if (_ballCountOnGround + _collectedBallCount == _currentLevelData.ballCount)
            {
                isLevelFinished = true;
                uiController.ShowStateUI(false);
                Debug.Log("LEVEL FAILED");
 
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

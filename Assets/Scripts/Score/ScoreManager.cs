using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Action<ScoreManager> OnScoreUpdated { get; set; }
    public int Multiplier { get; private set; } = 1;
    public int Score { get; private set; }
    public int BestScore { get; private set; }
    public int TotalPoints { get; private set; }
    public int RewardPoints { get; private set; }
    public int RewardMaxPoints { get { return scoreBarMaxPoints; } }

    [SerializeField]
    [Tooltip("When reaching a new multiplier, the next one is calculated based on this.")]
    private float scoreMultiplierStep = 1.1f;

    [SerializeField]
    [Tooltip("Amount of progress for the next multiplier each time the player scores a point.")]
    private float multiplierProgress = 0.5f;

    [SerializeField]
    private int scoreBarMaxPoints = 6;

    private const int BASE_SCORE = 1;
    private const int BASE_POINTS = 3;
    private const int BASE_SCORE_MULTIPLIER = 3;
    private const int BASE_MULTIPLIER_STEP = 1;
    private float currentMultiplierProgress;
    private float nextMultiplierStep;

    private void Start()
    {
        GameManager.Instance.OnPaperScored += UpdateScore;
        GameManager.Instance.OnPaperFailScore += ResetScore;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPaperScored -= UpdateScore;
        GameManager.Instance.OnPaperFailScore -= ResetScore;
    }

    private void UpdateScore()
    {
        int scoredPoints = BASE_SCORE * Multiplier;

        RewardPoints++;
        Score += scoredPoints;
        TotalPoints += scoredPoints * BASE_POINTS;

        if (Score > BestScore)
        {
            BestScore = Score;
        }

        if (RewardPoints >= RewardMaxPoints)
        {
            RewardPoints = 0;
            GrantReward();
        }

        CalculateMultiplier();
        OnScoreUpdated?.Invoke(this);
    }

    private void CalculateMultiplier()
    {
        currentMultiplierProgress += multiplierProgress;

        if (currentMultiplierProgress >= nextMultiplierStep)
        {
            currentMultiplierProgress = 0.0f;
            nextMultiplierStep *= scoreMultiplierStep;
            Multiplier++;
        }
    }

    private void ResetScore()
    {
        Score = 0;
        Multiplier = 1;
        currentMultiplierProgress = 0.0f;
        nextMultiplierStep = BASE_MULTIPLIER_STEP;
        OnScoreUpdated?.Invoke(this);
    }

    private void GrantReward()
    {

    }
}

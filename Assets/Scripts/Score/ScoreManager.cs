using System;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public Action<ScoreManager> OnScoreUpdated { get; set; }
    /// <summary>
    /// Multiplier used when a new point is scored.
    /// </summary>
    public int Multiplier { get; private set; } = 1;
    /// <summary>
    /// Live score without failures.
    /// </summary>
    public int Score { get; private set; }
    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }
    /// <summary>
    /// Points accumulated during all gameplay.
    /// </summary>
    public int TotalPoints { get; private set; }
    public int RewardPoints { get; private set; }
    public int RewardMaxPoints { get { return scoreBarMaxPoints; } }

    [Header("Multiplier")]

    [SerializeField]
    [Tooltip("When reaching a new multiplier, the next one is calculated based on this.")]
    private float scoreMultiplierStep = 1.1f;

    [SerializeField]
    [Tooltip("Amount of progress for the next multiplier each time the player scores a point.")]
    private float multiplierProgress = 0.5f;

    [SerializeField]
    private int scoreBarMaxPoints = 6;

    [Header("Reward Event")]

    [SerializeField]
    private UnityEvent rewardGranted;

    private const int BASE_SCORE = 1;
    private const int BASE_POINTS = 3;
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

    /// <summary>
    /// Updates the game score and calculates the current multiplier.
    /// </summary>
    private void UpdateScore()
    {
        int scoredPoints = BASE_SCORE * Multiplier;
        int points = scoredPoints * BASE_POINTS;

        RewardPoints++;
        Score += scoredPoints;
        CurrentScore = points;
        TotalPoints += points;

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

    /// <summary>
    /// Calculates the multiplier progress.
    /// </summary>
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

    /// <summary>
    /// Resets the score and multiplier.
    /// </summary>
    private void ResetScore()
    {
        CurrentScore = 0;
        Score = 0;
        Multiplier = 1;
        currentMultiplierProgress = 0.0f;
        nextMultiplierStep = BASE_MULTIPLIER_STEP;
        OnScoreUpdated?.Invoke(this);
    }

    private void GrantReward()
    {
        rewardGranted?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinScoreController : MonoBehaviour
{
    public Action<int> OnScore { get; set; }

    private int scoreMultiplier = 1;
    private int baseScore = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PaperController paperController))
        {
            AddScore(paperController);
        }
    }

    public void SetScoreMultiplier(int multiplier)
    {
        scoreMultiplier = multiplier;
    }

    private void AddScore(PaperController paperController)
    {
        OnScore?.Invoke(baseScore * scoreMultiplier);
    }
}

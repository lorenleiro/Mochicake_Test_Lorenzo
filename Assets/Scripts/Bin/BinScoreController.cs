using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BinScoreController : MonoBehaviour
{
    public Action<int> OnScore { get; set; }

    [SerializeField]
    private ParticleSystem scoreEffect;

    [SerializeField]
    private UnityEvent onPaperScored;

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
        onPaperScored?.Invoke();
        OnScore?.Invoke(baseScore * scoreMultiplier);
    }
}

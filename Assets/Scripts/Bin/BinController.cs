using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinController : MonoBehaviour
{
    public Action<int> OnPaperScored { get; set; }

    [SerializeField] private BinScoreController scoreControllers;

    private void Awake()
    {
        RegisterScoreController();
    }

    private void OnDestroy()
    {
        UnregisterScoreController();
    }

    private void RegisterScoreController()
    {
        scoreControllers.OnScore += Scored;
    }

    private void UnregisterScoreController()
    {
        scoreControllers.OnScore -= Scored;
    }

    private void Scored(int score)
    {
        OnPaperScored?.Invoke(score);
    }
}

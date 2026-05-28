using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanManager : MonoBehaviour
{
    public Action<FanController> OnFanUpdated { get; set; }
    public FanController FanController { get { return fanController; } }

    [SerializeField]
    private FanController fanController;

    private void Start()
    {
        GameManager.Instance.OnPaperSpawned += PaperSpawned;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPaperSpawned -= PaperSpawned;
    }

    private void PaperSpawned(PaperController paperController)
    {
        fanController.SetPaper(paperController);
        OnFanUpdated?.Invoke(fanController);
    }
}

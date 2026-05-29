using System;
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

    public void DisableFan()
    {
        fanController.Disable();
        OnFanUpdated?.Invoke(fanController);

    }

    private void PaperSpawned(PaperController paperController)
    {
        fanController.SetPaper(paperController);
        OnFanUpdated?.Invoke(fanController);
    }
}

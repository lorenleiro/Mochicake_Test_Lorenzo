using System;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    public Action<PaperController> OnPaperSpawned { get; set; }
    public Action OnPaperScored { get; set; }
    public Action OnPaperFailScore { get; set; }
    public PaperController CurrentPaper { get { return currentPaper; } }

    [SerializeField]
    private GameSettingsData gameSettingsData;

    [SerializeField]
    private PaperPool paperPool;

    [SerializeField]
    private TrashController binController;

    [SerializeField]
    private FanController fanController;

    private PaperController currentPaper;

    private void Start()
    {
        GameManager.Instance.OnGameStart += RequestPaper;
        gameSettingsData.OnPaperVisualChanged += ChangePaperVisual;
    }

    private void OnDestroy()
    {
        Dispose();
    }

    /// <summary>
    /// Request a new paper from the pool and initializes it.
    /// </summary>
    private void RequestPaper()
    {
        if (currentPaper != null)
        {
            currentPaper.OnThrowFinished -= RequestPaper;
            currentPaper.OnPaperOutsideRange -= Dispose;

            if (currentPaper.Scored)
            {
                paperPool.Release(currentPaper);
                OnPaperScored?.Invoke();
            }
            else
            {
                OnPaperFailScore?.Invoke();
            }
        }

        currentPaper = paperPool.Get();
        currentPaper.InitializePaper(binController);
        currentPaper.OnThrowFinished += RequestPaper;
        currentPaper.OnPaperOutsideRange += Dispose;
        OnPaperSpawned?.Invoke(currentPaper);
    }

    private void Dispose()
    {
        if (currentPaper != null)
        {
            paperPool.Release(currentPaper);
        }

        GameManager.Instance.OnGameStart -= RequestPaper;
        gameSettingsData.OnPaperVisualChanged -= ChangePaperVisual;
    }

    /// <summary>
    /// Sets the new paper visual object.
    /// </summary>
    private void ChangePaperVisual()
    {
        if (currentPaper != null)
        {
            currentPaper.SetVisual(gameSettingsData.PaperVisuals[gameSettingsData.SelectedPaperVisual]);
        }
    }
}

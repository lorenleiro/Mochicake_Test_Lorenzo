using System;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    public Action<PaperController> OnPaperSpawned { get; set; }
    public Action OnPaperScored{ get; set; }
    public Action OnPaperFailScore{ get; set; }
    public PaperController CurrentPaper { get { return currentPaper; } }

    [SerializeField]
    private PaperPool paperPool;

    [SerializeField]
    private BinController binController;

    [SerializeField]
    private FanController fanController;

    private PaperController currentPaper;

    private void Start()
    {
        GameManager.Instance.OnGameStart += RequestPaper;
    }

    private void OnDestroy()
    {
        Dispose();
    }

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
    }
}

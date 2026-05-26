using UnityEngine;

public class PaperManager : MonoBehaviour
{
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
        InitializeGame();
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
            }
        }

        currentPaper = paperPool.Get();
        currentPaper.InitializePaper(binController);
        currentPaper.OnThrowFinished += RequestPaper;
        currentPaper.OnPaperOutsideRange += Dispose;
        fanController.SetPaper(currentPaper);
    }

    private void InitializeGame()
    {
        RequestPaper();
    }

    private void Dispose()
    {
        if (currentPaper != null)
        {
            paperPool.Release(currentPaper);
        }
    }
}

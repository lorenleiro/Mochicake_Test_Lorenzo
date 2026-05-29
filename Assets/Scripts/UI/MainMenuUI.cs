using UnityEngine.UIElements;

public class MainMenuUI : BaseUI
{
    private Button playButton;
    private Button exitButton;

    public MainMenuUI(VisualElement root) : base()
    {
        playButton = root.Q<Button>("play_button");
        exitButton = root.Q<Button>("exit_button");

        playButton.clicked += StartGame;
        exitButton.clicked += ExitGame;
    }

    public override void Dispose()
    {
        playButton.clicked -= StartGame;
        exitButton.clicked -= ExitGame;
    }

    private void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    private void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }
}

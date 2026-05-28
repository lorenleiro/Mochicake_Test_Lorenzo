using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainGameUI : MonoBehaviour
{
    [SerializeField]
    private GameSettingsData gameSettingsData;

    private VisualElement root;
    private VisualElement fanRoot;
    private VisualElement gameplayRoot;
    private VisualElement mainMenuRoot;
    private BaseUI fanUI;
    private BaseUI gameplayUI;
    private BaseUI mainMenuUI;

    private void Start()
    {
        InitializeUI();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private void InitializeUI()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        fanRoot = root.Q<VisualElement>("FanUI");
        gameplayRoot = root.Q<VisualElement>("GameplayUI");
        mainMenuRoot = root.Q<VisualElement>("MainMenuUI");

        fanUI = new FanUI(fanRoot);
        gameplayUI = new GameplayUI(gameplayRoot, gameSettingsData);
        mainMenuUI = new MainMenuUI(mainMenuRoot);

        fanRoot.style.display = DisplayStyle.None;
        gameplayRoot.style.display = DisplayStyle.None;
        mainMenuRoot.style.display = DisplayStyle.Flex;

        GameManager.Instance.OnGameStart += StartGame;
    }

    private void Dispose()
    {
        fanUI?.Dispose();
        gameplayUI?.Dispose();
        mainMenuUI?.Dispose();

        GameManager.Instance.OnGameStart -= StartGame;
    }

    private void StartGame()
    {
        fanRoot.style.display = DisplayStyle.Flex;
        gameplayRoot.style.display = DisplayStyle.Flex;
        mainMenuRoot.style.display = DisplayStyle.None;
    }
}

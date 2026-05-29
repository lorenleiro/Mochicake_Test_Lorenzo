using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FanUI : BaseUI
{
    private VisualElement leftForceContainer;
    private VisualElement rightForceContainer;
    private Label scoreText;
    private Label fanForceText;
    private Coroutine movePointCO;
    private Vector2 scoreStartPosition = new Vector2(75.0f, -500.0f);

    public FanUI(VisualElement root) : base()
    {
        leftForceContainer = root.Q<VisualElement>("left_force_container");
        rightForceContainer = root.Q<VisualElement>("right_force_container");
        fanForceText = root.Q<Label>("fan_force_value_text");
        scoreText = root.Q<Label>("score_text");

        GameManager.Instance.OnFanUpdated += FanUpdated;
        GameManager.Instance.OnScoreUpdated += ShowScorePoint;
    }

    public override void Dispose()
    {
        GameManager.Instance.OnFanUpdated -= FanUpdated;
        GameManager.Instance.OnScoreUpdated -= ShowScorePoint;
    }

    private void FanUpdated(FanController fanController)
    {
        SetForceAndDirection(fanController.VisualForce, fanController.Direction);
    }

    private void SetForceAndDirection(float force, FanDirectionEnum direction)
    {
        fanForceText.text = force.ToString("F2");

        switch (direction)
        {
            case FanDirectionEnum.None:
                {
                    leftForceContainer.style.display = DisplayStyle.None;
                    rightForceContainer.style.display = DisplayStyle.None;
                    fanForceText.text = "0.0";
                }
                break;
            case FanDirectionEnum.Left:
                {
                    leftForceContainer.style.display = DisplayStyle.Flex;
                    rightForceContainer.style.display = DisplayStyle.None;
                }
                break;
            case FanDirectionEnum.Right:
                {
                    rightForceContainer.style.display = DisplayStyle.Flex;
                    leftForceContainer.style.display = DisplayStyle.None;
                }
                break;
        }
    }

    private void ShowScorePoint(ScoreManager scoreManager)
    {
        if (scoreManager.CurrentScore <= 0)
        {
            return;
        }

        if (movePointCO != null)
        {
            GameManager.Instance.StopCoroutine(movePointCO);
        }

        movePointCO = GameManager.Instance.StartCoroutine(MoveScoreAndFade(scoreText, scoreManager.CurrentScore, scoreStartPosition, 2.0f));
    }

    private IEnumerator MoveScoreAndFade(Label element, int score, Vector2 startScreenPos, float duration, float moveSpeed = 100f, float fadeSpeed = 1f)
    {
        float time = 0f;

        element.style.position = Position.Absolute;
        element.style.left = startScreenPos.x;
        element.style.top = startScreenPos.y;
        element.style.opacity = 1f;
        element.style.display = DisplayStyle.Flex;
        element.text = $"+{score}";

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            element.style.top = startScreenPos.y - t * moveSpeed;
            element.style.opacity = 1f - t * fadeSpeed;

            yield return null;
        }

        element.style.display = DisplayStyle.None;
    }
}

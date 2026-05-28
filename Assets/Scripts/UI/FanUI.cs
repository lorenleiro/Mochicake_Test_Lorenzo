using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FanUI : BaseUI
{
    private VisualElement root;
    private VisualElement leftForceContainer;
    private VisualElement rightForceContainer;
    private Label fanForceText;

    public FanUI(VisualElement root) : base()
    {
        this.root = root;

        fanForceText = root.Q<Label>("fan_force_value_text");
        leftForceContainer = root.Q<VisualElement>("left_force_container");
        rightForceContainer = root.Q<VisualElement>("right_force_container");

        GameManager.Instance.OnFanUpdated += FanUpdated;
    }

    public override void Dispose()
    {
        GameManager.Instance.OnFanUpdated -= FanUpdated;
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
}

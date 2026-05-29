using UnityEngine;
using UnityEngine.InputSystem;

public class PaperInputController : MonoBehaviour
{
    [SerializeField]
    private PaperManager paperManager;

    [SerializeField]
    [Tooltip("Amount of force used to push the paper forward.")]
    private float throwForce = 2.0f;

    [SerializeField]
    [Tooltip("Amount of force used to push the paper upwards.")]
    private float throwForceUpwards = 0.5f;

    [SerializeField]
    [Tooltip("Maximum angle the player can throw.")]
    private float maxThrowAngle = 30.0f;

    [SerializeField]
    [Tooltip("The greater the throw angle, the greater the force to compensate the extra distance the paper needs to travel.")]
    private float maxForceCompensation = 1.2f;

    [SerializeField]
    [Tooltip("Minimum vector distance needed to throw the paper.")]
    private float minDragDistance = 0.5f;

    private GameInput gameInput;
    private PaperController paperController;
    private Vector3 throwStartPos;

    private void Awake()
    {
        RegisterInput();
    }

    private void OnDestroy()
    {
        UnregisterInput();
    }

    private void RegisterInput()
    {
        gameInput = new GameInput();
        gameInput.PaperControls.Throw.started += PrepareThrow;
        gameInput.PaperControls.Throw.canceled += ThrowPaper;
        gameInput.Enable();
    }

    private void UnregisterInput()
    {
        gameInput.PaperControls.Throw.started -= PrepareThrow;
        gameInput.PaperControls.Throw.canceled -= ThrowPaper;
        gameInput.Disable();
    }

    private void PrepareThrow(InputAction.CallbackContext ctx)
    {
        throwStartPos = Mouse.current.position.ReadValue();
    }

    private void ThrowPaper(InputAction.CallbackContext ctx)
    {
        paperController = paperManager.CurrentPaper;

        if (paperController == null)
        {
            return;
        }

        Vector2 mouseEnd = Mouse.current.position.ReadValue();
        Vector2 drag = mouseEnd - (Vector2)throwStartPos;

        if (drag.sqrMagnitude < minDragDistance * minDragDistance)
        {
            return;
        }

        Vector3 forward = paperController.transform.forward;
        Vector3 right = paperController.transform.right;
        Vector3 horizontalDirection = (forward + right * drag.x * 0.01f).normalized;

        float angle = Vector3.Angle(forward, horizontalDirection);
        float forceCompensation = Mathf.Clamp(1f / Mathf.Cos(angle * Mathf.Deg2Rad), 1f, maxForceCompensation);

        horizontalDirection = ClampDirection(horizontalDirection);
        float compensatedForce = throwForce * forceCompensation;
        Vector3 finalForce = horizontalDirection * compensatedForce + Vector3.up * throwForceUpwards;

        paperController.Launch(finalForce);
    }

    /// <summary>
    /// Clamps the provided vector inside the specified MaxThrowAngle.
    /// </summary>
    /// <param name="direction">Vector to clamp.</param>
    /// <returns>The clamped vector.</returns>
    private Vector3 ClampDirection(Vector3 direction)
    {
        Vector3 forwardFlat = Vector3.ProjectOnPlane(paperController.transform.forward, Vector3.up).normalized;
        Vector3 directionFlat = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;

        float angle = Vector3.Angle(forwardFlat, directionFlat);

        if (angle > maxThrowAngle)
        {
            direction = Vector3.RotateTowards(forwardFlat, directionFlat, maxThrowAngle * Mathf.Deg2Rad, 0f);
        }

        direction.y += throwForceUpwards;

        return direction.normalized;
    }
}

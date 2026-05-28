using UnityEngine;
using UnityEngine.InputSystem;

public class PaperInputController : MonoBehaviour
{
    [SerializeField]
    private PaperManager paperManager;

    [SerializeField]
    private float throwForce = 2.0f;

    [SerializeField]
    private float throwForceUpwards = 0.5f;

    [SerializeField]
    private float maxThrowAngle = 30.0f;

    [SerializeField] 
    private float maxForceCompensation = 1.2f;

    [SerializeField]
    private float minDragDistance= 0.5f;

    private GameInput gameInput;
    private PaperController paperController;
    private Camera mainCamera;
    private Vector3 throwStartPos;
    private Vector3 throwEndPos;

    private void Awake()
    {
        RegisterInput();
        mainCamera = Camera.main;
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
        if (paperController == null) return;

        Vector2 mouseEnd = Mouse.current.position.ReadValue();
        Vector2 drag = mouseEnd - (Vector2)throwStartPos;

        if(drag.sqrMagnitude < minDragDistance * minDragDistance)
        {
            return;
        }

        Vector3 forward = paperController.transform.forward;
        Vector3 right = paperController.transform.right;

        Vector3 horizontalDirection = (forward + right * drag.x * 0.01f).normalized;

        // Compensacion antes del clamp con la direccion real del raton
        float angle = Vector3.Angle(forward, horizontalDirection);
        float forceCompensation = Mathf.Clamp(1f / Mathf.Cos(angle * Mathf.Deg2Rad), 1f, maxForceCompensation);

        // Clamp despues
        horizontalDirection = ClampDirection(horizontalDirection);
        float compensatedForce = throwForce * forceCompensation;
        Vector3 finalForce = horizontalDirection * compensatedForce + Vector3.up * throwForceUpwards;

        paperController.Launch(finalForce);
    }

    private Vector3 ClampDirection(Vector3 direction)
    {
        // Clamp solo en el plano horizontal ignorando Y
        Vector3 forwardFlat = Vector3.ProjectOnPlane(paperController.transform.forward, Vector3.up).normalized;
        Vector3 directionFlat = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;

        float angle = Vector3.Angle(forwardFlat, directionFlat);

        if (angle > maxThrowAngle)
            direction = Vector3.RotateTowards(forwardFlat, directionFlat, maxThrowAngle * Mathf.Deg2Rad, 0f);

        direction.y += throwForceUpwards;
        return direction.normalized;
    }
}

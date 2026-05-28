using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PaperController : MonoBehaviour
{
    public Action OnThrowFinished { get; set; }
    public Action OnPaperOutsideRange { get; set; }
    public Rigidbody Body { get; private set; }
    public Vector3 LaunchDirection { get; private set; }

    /// <summary>
    /// Tells if this paper can receive collision callbacks.
    /// </summary>
    public bool RegisterCollisions { get; set; } = true;
    public bool Launched { get; private set; }
    public bool Scored { get; set; }

    [SerializeField]
    private float stopThreshold = 0.1f;

    [Tooltip("Distance from this paper to the bin at which the paper is no longer visible in the main camera, so it can be released.")]
    [SerializeField]
    private float releaseDistance = 5.0f;

    [Tooltip("Sets the amount of drag the paper will have when colliding with something.")]
    [SerializeField]
    private float slowDownThreshold = 5.0f;

    [SerializeField]
    private float launchRotationForce = 5.0f;

    [SerializeField]
    private float dragOnTouch = 1.0f;
    [SerializeField]
    private float upwardForce = 5.0f;

    [SerializeField]
    private string scoreColliderTag;

    private BinController binController;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!RegisterCollisions)
        {
            StopMove();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!RegisterCollisions)
        {
            return;
        }

        if (other.CompareTag(scoreColliderTag))
        {
            Scored = true;
        }

        CheckDistance();

        RegisterCollisions = false;
        OnThrowFinished?.Invoke();
    }

    public void InitializePaper(BinController binController)
    {
        this.binController = binController;
        Launched = false;
    }

    public void Launch(Vector3 force)
    {
        LaunchDirection = force.normalized;
        Body.isKinematic = false;
        Body.useGravity = true;
        Body.AddForce(force, ForceMode.Impulse);
        Body.AddTorque(transform.right * launchRotationForce, ForceMode.Impulse);
        Launched = true;
    }

    private void StopMove()
    {
        if(Body.drag == 0.0f)
        {
            Body.drag = dragOnTouch;
        }

        Body.velocity = Vector3.MoveTowards(Body.velocity, Vector3.zero, slowDownThreshold * Time.fixedDeltaTime);
        Body.angularVelocity = Vector3.MoveTowards(Body.angularVelocity, Vector3.zero, slowDownThreshold * Time.fixedDeltaTime);

        if (!Body.isKinematic && Body.velocity.magnitude < stopThreshold)
        {
            Body.velocity = Vector3.zero;
            Body.angularVelocity = Vector3.zero;
            Body.Sleep();
        }
    }

    private void CheckDistance()
    {
        float sqrDistance = (transform.position - binController.transform.position).sqrMagnitude;

        if (sqrDistance > releaseDistance)
        {
            OnPaperOutsideRange?.Invoke();
        }
    }
}

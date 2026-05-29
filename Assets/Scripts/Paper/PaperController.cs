using System;
using UnityEngine;

public class PaperController : MonoBehaviour
{
    public Action OnThrowFinished { get; set; }
    /// <summary>
    /// Called when the paper is thrown far away, so the pool can retrieve it.
    /// </summary>
    public Action OnPaperOutsideRange { get; set; }
    public Rigidbody Body { get; private set; }
    public TrailRenderer TrailRenderer { get { return trailRenderer; } }
    public Vector3 LaunchDirection { get; private set; }

    /// <summary>
    /// Tells if this paper can receive collision callbacks.
    /// </summary>
    public bool RegisterCollisions { get; set; } = true;
    /// <summary>
    /// If this paper is mid-air.
    /// </summary>
    public bool Launched { get; private set; }
    /// <summary>
    /// If this paper, when collided with anything, did score.
    /// </summary>
    public bool Scored { get; set; }

    [Header("Collision Stop Settings")]

    [SerializeField]
    private float stopThreshold = 0.1f;

    [SerializeField]
    [Tooltip("Sets the amount of drag the paper will have when colliding with something.")]
    private float slowDownThreshold = 5.0f;

    [SerializeField]
    [Tooltip("Distance from this paper to the bin at which the paper is no longer visible in the main camera, so it can be released.")]
    private float releaseDistance = 5.0f;

    [SerializeField]
    private float dragOnTouch = 1.0f;

    [Header("Other Settings")]

    [SerializeField]
    [Tooltip("Forced used to make the paper spin when launched.")]
    private float launchRotationForce = 5.0f;

    [SerializeField]
    private string scoreColliderTag;

    [Header("Visuals")]

    [SerializeField]
    private GameObject currentVisual;

    [SerializeField]
    private TrailRenderer trailRenderer;

    [Header("Sound Settings")]

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip paperHitClip;

    [SerializeField]
    private AudioClip paperLaunchClip;

    private TrashController binController;


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

        if (gameObject.activeSelf)
        {
            audioSource.PlayOneShot(paperHitClip);
        }

        RegisterCollisions = false;
        OnThrowFinished?.Invoke();
    }

    public void InitializePaper(TrashController binController)
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
        trailRenderer.emitting = true;
        audioSource.PlayOneShot(paperLaunchClip);
        Launched = true;
    }

    /// <summary>
    /// Destroys the current visual object and creates a new one.
    /// </summary>
    /// <param name="visual"></param>
    public void SetVisual(GameObject visual)
    {
        if (currentVisual != null)
        {
            Destroy(currentVisual);
        }

        currentVisual = Instantiate(visual);
        currentVisual.transform.SetParent(transform);
        currentVisual.transform.position = transform.position;
    }

    /// <summary>
    /// Makes the paper slowly stop when colliding with anything.
    /// </summary>
    private void StopMove()
    {
        if (Body.drag == 0.0f)
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

    /// <summary>
    /// Checks if this paper is to far away from the trash so the pool can retrieve it.
    /// </summary>
    private void CheckDistance()
    {
        float sqrDistance = (transform.position - binController.transform.position).sqrMagnitude;

        if (sqrDistance > releaseDistance)
        {
            OnPaperOutsideRange?.Invoke();
        }
    }
}

using UnityEngine;

public class FanController : MonoBehaviour
{
    public FanDirectionEnum Direction { get; private set; }
    public float VisualForce { get; private set; }

    [SerializeField]
    private GameObject fanGO;

    [SerializeField]
    private Transform fanLeftPosition;

    [SerializeField]
    private Transform fanRightPosition;

    [SerializeField]
    [Tooltip("Minimum force to apply to the paper mid-air.")]
    private float minWindForce;

    [SerializeField]
    [Tooltip("Maximum force to apply to the paper mid-air.")]
    private float maxWindForce;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("Scale of the calculated min-max force values.")]
    private float forceScale;

    private PaperController paper;
    private Vector3 windDirection;
    private float realForce;
    private bool disabled;

    private void FixedUpdate()
    {
        if (paper == null)
        {
            return;
        }

        ApplyForces();
    }

    /// <summary>
    /// Sets the paper that will be pushed by this fan.
    /// </summary>
    /// <param name="paper">The paper that will be pushed.</param>
    public void SetPaper(PaperController paper)
    {
        if (!fanGO.activeSelf)
        {
            fanGO.SetActive(true);
        }

        this.paper = paper;
        disabled = false;

        SetRandomWindForce();
    }

    /// <summary>
    /// Disables completely the fan, so the paper it's not influenced by it.
    /// </summary>
    public void Disable()
    {
        fanGO.SetActive(false);
        realForce = 0.0f;
        Direction = FanDirectionEnum.None;
        disabled = true;
    }

    /// <summary>
    /// Calculates a random wind force and direction.
    /// </summary>
    private void SetRandomWindForce()
    {
        float randomDirection = Random.Range(0.0f, 1.0f);

        if (randomDirection > 0.5f)
        {
            fanGO.transform.position = fanRightPosition.position;
            fanGO.transform.rotation = fanRightPosition.transform.rotation;
            windDirection = transform.right;
            Direction = FanDirectionEnum.Right;
        }
        else
        {
            fanGO.transform.position = fanLeftPosition.position;
            fanGO.transform.rotation = fanLeftPosition.transform.rotation;
            windDirection = -transform.right;
            Direction = FanDirectionEnum.Left;
        }

        VisualForce = Random.Range(minWindForce, maxWindForce);
        realForce = VisualForce * forceScale;
        paper.Body.AddForce(windDirection * realForce, ForceMode.Force);
    }

    /// <summary>
    /// Applies side forces to the current paper only on it's lateral axis.
    /// </summary>
    private void ApplyForces()
    {
        if (paper == null || !paper.Launched || disabled)
        {
            return;
        }

        float launchSpeed = Vector3.Dot(paper.Body.velocity, paper.LaunchDirection);

        paper.Body.AddForce(windDirection * realForce, ForceMode.Force);

        Vector3 lateralVelocity = paper.Body.velocity - paper.LaunchDirection * launchSpeed;
        paper.Body.velocity = paper.LaunchDirection * launchSpeed + lateralVelocity;
    }
}

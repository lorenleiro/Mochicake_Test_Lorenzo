using System.Collections;
using System.Collections.Generic;
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
    private float minWindForce;

    [SerializeField]
    private float maxWindForce;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float forceScale;

    private PaperController paper;
    private Vector3 windDirection;
    private float realForce;

    private void FixedUpdate()
    {
        if (paper == null)
        {
            return;
        }

        ApplyForces();
    }

    public void SetPaper(PaperController paper)
    {
        this.paper = paper;
        SetRandomWindForce();
    }

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
        if (paper == null || !paper.Launched) return;

        float launchSpeed = Vector3.Dot(paper.Body.velocity, paper.LaunchDirection);

        paper.Body.AddForce(windDirection * realForce, ForceMode.Force);

        Vector3 lateralVelocity = paper.Body.velocity - paper.LaunchDirection * launchSpeed;
        paper.Body.velocity = paper.LaunchDirection * launchSpeed + lateralVelocity;
    }
}

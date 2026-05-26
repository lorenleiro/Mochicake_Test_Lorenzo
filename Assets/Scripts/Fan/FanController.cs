using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour
{
    [SerializeField]
    private Transform applyForce;

    [SerializeField]
    private float minWindForce;

    [SerializeField]
    private float maxWindForce;

    private PaperController paper;
    private Vector3 windDirection;
    private float windForce;

    private void FixedUpdate()
    {
        if (paper == null)
        {
            return;
        }

        ApplyForces();
    }

    public void SetRandomWindForce()
    {
        float randomDirection = Random.Range(0.0f, 1.0f);
        windDirection = randomDirection > 0.5f ? transform.right : -transform.right;

        paper.Body.AddForce(windDirection * windForce, ForceMode.Force);
        windForce = Random.Range(minWindForce, maxWindForce);
    }

    public void SetPaper(PaperController paper)
    {
        this.paper = paper;
        SetRandomWindForce();
    }

    private void ApplyForces()
    {
        if (paper.Launched)
        {
            paper.Body.AddForce(windDirection * windForce, ForceMode.Force);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    public Action<int> OnPaperScored { get; set; }

    [SerializeField]
    private BinScoreController scoreControllers;

    [Header("Upgrade Settings")]

    [SerializeField]
    [Tooltip("When a paper hits the trash, the amount of time to reset to it's original size.")]
    private float upgradeResetTime = 1.0f;

    [SerializeField]
    [Tooltip("New scale when the trash is upgraded.")]
    private Vector3 upgradeScale;

    [Header("Audio Settings")]

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip binHitClip;

    [SerializeField]
    private AudioClip paperScoredClip;

    private Coroutine resetTrashCO;
    private Vector3 originalScale;

    private void Awake()
    {
        RegisterScoreController();
        originalScale = transform.localScale;
    }

    private void OnDestroy()
    {
        UnregisterScoreController();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out PaperController paper))
        {
            audioSource.PlayOneShot(binHitClip);

            if (resetTrashCO != null)
            {
                StopCoroutine(resetTrashCO);
            }

            resetTrashCO = StartCoroutine(ResetTrashSize());
        }
    }

    public void Upgrade()
    {
        transform.localScale = upgradeScale;
    }

    private void RegisterScoreController()
    {
        scoreControllers.OnScore += Scored;
    }

    private void UnregisterScoreController()
    {
        scoreControllers.OnScore -= Scored;
    }

    private void Scored(int score)
    {
        audioSource.PlayOneShot(paperScoredClip);
        OnPaperScored?.Invoke(score);
        transform.localScale = originalScale;
    }

    private IEnumerator ResetTrashSize()
    {
        yield return new WaitForSeconds(upgradeResetTime);

        transform.localScale = originalScale;
    }
}

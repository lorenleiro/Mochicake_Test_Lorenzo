using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private float minPauseDuration = 2f;

    [SerializeField]
    private float maxPauseDuration = 6f;

    [SerializeField]
    private int minFlicks = 2;

    [SerializeField]
    private int maxFlicks = 8;

    [SerializeField]
    private Vector2 minMaxFlicksTime = new Vector2(0.03f, 0.1f);

    private Light lightComponent;

    private void Awake()
    {
        lightComponent = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPauseDuration, maxPauseDuration));

            int flicks = Random.Range(minFlicks, maxFlicks);

            for (int i = 0; i < flicks; i++)
            {
                lightComponent.enabled = !lightComponent.enabled;

                yield return new WaitForSeconds(Random.Range(minMaxFlicksTime.x, minMaxFlicksTime.y));
            }

            lightComponent.enabled = true;
        }
    }
}
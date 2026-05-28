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
    private int maxFlicks = 6;

    private Light light;

    private void Awake()
    {
        light = GetComponent<Light>();
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
                light.enabled = !light.enabled;
                yield return new WaitForSeconds(Random.Range(0.03f, 0.1f));
            }

            light.enabled = true;
        }
    }
}
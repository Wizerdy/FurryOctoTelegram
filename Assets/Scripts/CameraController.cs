using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public AnimationCurve lowIntensity;
    public AnimationCurve medIntensity;
    public AnimationCurve highIntensity;
    public float lowDuration;
    public float medDuration;
    public float highDuration;

    Coroutine shaking;

    public void OnPlayerCollision()
    {
        Shake(medDuration, medIntensity);
    }
    public void OnEnemyDestroyed()
    {
        Shake(lowDuration, lowIntensity);
    }
    public void OnPlayerDestroyed()
    {
        Shake(highDuration, highIntensity);
    }

    void Shake(float duration, AnimationCurve intensity)
    {
        if (shaking == null)
        {
            shaking = StartCoroutine(OnShake(duration, intensity));
        }
    }

    IEnumerator OnShake(float duration, AnimationCurve intensity)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity.Evaluate(elapsed);
            float y = Random.Range(-1f, 1f) * intensity.Evaluate(elapsed);
            float z = Random.Range(-1f, 1f) * intensity.Evaluate(elapsed);

            transform.localPosition = new Vector3(x, y, z - 30);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        shaking = null;
    }
}

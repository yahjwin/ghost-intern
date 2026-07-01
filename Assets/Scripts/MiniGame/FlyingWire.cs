using System.Collections;
using UnityEngine;

public class FlyingWire : MonoBehaviour
{
    public float flyTime = 0.6f;
    public float arcHeight = 1.2f;

    public void FlyTo(Vector3 targetPosition)
    {
        StartCoroutine(FlyRoutine(targetPosition));
    }

    IEnumerator FlyRoutine(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < flyTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flyTime;

            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            transform.position = currentPos;

            transform.Rotate(0f, 0f, 720f * Time.deltaTime);

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = Quaternion.identity;
    }
}
using UnityEngine;
using System.Collections;

public class AutolaunchingForCatching : MonoBehaviour
{
    public GameObject prefabToThrow;
    public Transform characterTransform;
    public float throwForce = 10f;
    public float throwInterval = 5f;
    public bool throwEnabled = true;
    public float arcHeight = 3f;

    private float timeSinceLastThrow = 0f;

    void Update()
    {
        if (throwEnabled)
        {
            timeSinceLastThrow += Time.deltaTime;

            if (timeSinceLastThrow >= throwInterval)
            {
                ThrowPrefabTowardsCharacter();
                timeSinceLastThrow = 0f;
            }
        }
    }

    void ThrowPrefabTowardsCharacter()
    {
        GameObject thrownObject = Instantiate(prefabToThrow, transform.position, transform.rotation);
        Vector3 throwDirection = (characterTransform.position - transform.position).normalized;
        float distanceToCharacter = Vector3.Distance(transform.position, characterTransform.position);
        float throwDuration = distanceToCharacter / throwForce;
        float maxArcHeight = arcHeight + distanceToCharacter / 10f;
        Vector3 arcMidPoint = transform.position + throwDirection * distanceToCharacter * 0.5f + Vector3.up * maxArcHeight;
        AnimationCurve arcCurve = AnimationCurve.EaseInOut(0f, 0f, throwDuration, 1f);

        StartCoroutine(ThrowObject(thrownObject, throwDirection, throwDuration, arcMidPoint, arcCurve));
    }

    IEnumerator ThrowObject(GameObject thrownObject, Vector3 throwDirection, float throwDuration, Vector3 arcMidPoint, AnimationCurve arcCurve)
    {
        float elapsedTime = 0f;

        while (elapsedTime < throwDuration)
        {
            float t = elapsedTime / throwDuration;
            Vector3 lerpedPosition = Vector3.Lerp(transform.position, characterTransform.position, t);
            float arcOffset = arcCurve.Evaluate(t) * arcHeight;
            Vector3 arcVector = arcMidPoint - Vector3.Lerp(transform.position, characterTransform.position, t);
            lerpedPosition += arcVector.normalized * arcOffset;
            thrownObject.transform.position = lerpedPosition;
            thrownObject.transform.LookAt(lerpedPosition + arcVector.normalized);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(thrownObject);
    }
}
using UnityEngine;

public class BallTrajectoryAdjustment : MonoBehaviour
{
    [SerializeField] public Transform target;
    private bool applyCurve = false;
    [SerializeField] private Transform player;
    private Vector3 initialVelocity;
    private Vector3 curveAcceleration;
    private Rigidbody ballRigidbody;
    [SerializeField] private float scaleFactor = 1f;
    [SerializeField] private float curveMax = .1f;

    private void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void CalculateCurveAcceleration()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 initialDirection = initialVelocity.normalized;
        float angle = Vector3.Angle(initialDirection, directionToTarget);

        Debug.Log("Angle: " + angle);

        if (angle <= 40f)
        {
            Debug.Log("Angle within range");

            float distanceToTarget = (transform.position - target.position).sqrMagnitude;
            float initialVelocityMagnitude = initialVelocity.magnitude;

            if (initialVelocityMagnitude == 0f)
            {
                Debug.LogWarning("Initial velocity is zero, curve acceleration calculation skipped.");
                curveAcceleration = Vector3.zero;
                return;
            }

            float timeToReachTarget = distanceToTarget / initialVelocityMagnitude;
            Vector3 finalBallPosition = transform.position + initialVelocity * timeToReachTarget;
            float distanceToFinalBallPosition = (finalBallPosition - target.position).sqrMagnitude;

            if (timeToReachTarget == 0f)
            {
                Debug.LogWarning("Time to reach target is zero, curve acceleration calculation skipped.");
                curveAcceleration = Vector3.zero;
                return;
            }

            float accelerationMagnitude = (2 * (distanceToFinalBallPosition - distanceToTarget) - initialVelocityMagnitude * timeToReachTarget) / (timeToReachTarget * timeToReachTarget);
            curveAcceleration = (directionToTarget * accelerationMagnitude) * scaleFactor;
            curveAcceleration = Vector3.ClampMagnitude(curveAcceleration, Mathf.Max(curveAcceleration.magnitude, curveMax));

            Debug.Log("Curve acceleration: " + curveAcceleration);
        }
        else
        {
            curveAcceleration = Vector3.zero;
        }
    }


    private void ApplyCurve()
    {
        if (curveAcceleration != Vector3.zero)
        {
            Debug.Log("Applying curve: " + curveAcceleration);
        }
        else
        {
            Debug.Log("Curve acceleration is zero");
        }

        ballRigidbody.AddForce(curveAcceleration * Time.deltaTime, ForceMode.Acceleration);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHands"))
        {
            Debug.Log("Exited PlayerHands");
            applyCurve = true;
            Debug.Log("applyCurve set to true");
            initialVelocity = ballRigidbody.velocity;
            CalculateCurveAcceleration();
        }
    }

    public void DisableCurve()
    {
        applyCurve = false;
    }

    private void Update()
    {
        if (applyCurve)
        {
            float distanceToTarget = (transform.position - target.position).sqrMagnitude;
            float distanceToPlayer = (transform.position - player.position).sqrMagnitude;

            if (distanceToPlayer > distanceToTarget)
            {
                Debug.Log("Disabling curve");
                applyCurve = false;
            }
            else
            {
                ApplyCurve();
            }
        }
    }
}

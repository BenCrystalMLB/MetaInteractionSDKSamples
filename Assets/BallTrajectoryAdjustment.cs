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

            Vector3 perpendicularPoint = Vector3.ProjectOnPlane(target.position - transform.position, initialDirection) + transform.position;
            float distanceToPerpendicularPoint = Vector3.Distance(transform.position, perpendicularPoint);
            float timeToReachPerpendicularPoint = distanceToPerpendicularPoint / initialVelocity.magnitude;

            // Change the required acceleration direction calculation
            Vector3 requiredAccelerationDirection = (directionToTarget - initialDirection).normalized;
            float requiredAccelerationMagnitude = 2 * Vector3.Distance(perpendicularPoint, target.position) / (timeToReachPerpendicularPoint * timeToReachPerpendicularPoint);

            Vector3 totalAcceleration = requiredAccelerationDirection * requiredAccelerationMagnitude;

            // Calculate the lateral acceleration
            Vector3 accelerationProjection = Vector3.Project(totalAcceleration, initialDirection);
            curveAcceleration = (totalAcceleration - accelerationProjection) * scaleFactor;
            curveAcceleration = Vector3.ClampMagnitude(curveAcceleration, Mathf.Max(curveAcceleration.magnitude, curveMax));


            // Print intermediate variables for debugging
            Debug.Log("Perpendicular Point: " + perpendicularPoint);
            Debug.Log("Distance to Perpendicular Point: " + distanceToPerpendicularPoint);
            Debug.Log("Time to Reach Perpendicular Point: " + timeToReachPerpendicularPoint);
            Debug.Log("Required Acceleration Direction: " + requiredAccelerationDirection);
            Debug.Log("Required Acceleration Magnitude: " + requiredAccelerationMagnitude);
            Debug.Log("Total Acceleration: " + totalAcceleration);
            Debug.Log("Acceleration Projection: " + accelerationProjection);
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
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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

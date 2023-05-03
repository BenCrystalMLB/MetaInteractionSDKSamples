/*
 * 1) Check the angle difference between the throw and the target to make sure it falls within a 40 degree threshold
 * 2) Calculate the distance between the player and the target
 * 3) Calculate the amount of time it would take for the ball to travel that distance given its initial velocity
 * 4) Calculate the distance between the final ball position at that time and the target
 * 5) Calculate a constant acceleration to be applied to the ball to get it to end up at the target's location by 
 *    altering the trajectory of the throw over the course of its flight path using the equation a = (2 * (B - A) - v * T) / T^2
 */



using UnityEngine;

public class BallTrajectoryAdjustmentManual : MonoBehaviour
{
    public Transform target;
    private bool isAccelerating = false; // the artist formerly known as applyCurve
    public Transform player;
    private Vector3 initialVelocity;
    private Vector3 accelerationForceVector; // the artist formerly known as curveAcceleration
    private Rigidbody ballRigidbody;
    public float scaleFactor = 1f;
    public float curveMax = .1f;
    public float maxAcceleratingDistance = 50f;

    private void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void CalculateAdjustmentAcceleration()
    {
        // Check the angle difference between the throw and the target to make sure it falls within a 40 degree threshold
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 initialDirection = initialVelocity.normalized;
        float angle = Vector3.Angle(initialDirection, directionToTarget);

        Debug.Log("Angle: " + angle);

        if (angle <= 40f)
        {

            // Calculate the distance between the player and the target
            Debug.Log("Angle within range");

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            float initialVelocityMagnitude = initialVelocity.magnitude;

            // If throw is invalid, return
            if (initialVelocityMagnitude == 0f)
            {
                Debug.LogWarning("Initial velocity is zero, acceleration calculation skipped.");
                accelerationForceVector = Vector3.zero;
                DisableAcceleration();
                return;
            }

            

            // Calculate the amount of time it would take for the ball to travel that distance given its initial velocity
            float timeToReachTarget = distanceToTarget / initialVelocityMagnitude;
            Vector3 finalBallPosition = transform.position + initialVelocity * timeToReachTarget;
            float distanceToFinalBallPosition = Vector3.Distance(finalBallPosition, target.position);

            if (timeToReachTarget == 0f)
            {
                Debug.LogWarning("Time to reach target is zero, acceleration calculation skipped.");
                accelerationForceVector = Vector3.zero;
                DisableAcceleration();
                return;
            }

            Debug.Log("Initial velocity: " + initialVelocity);
            Debug.Log("Time to reach target: "+ timeToReachTarget);

            // Calculate the distance between the final ball position at that time and the target
            // Calculate a constant acceleration to be applied to the ball to get it to end up at the target's location by 
            // altering the trajectory of the throw over the course of its flight path using the equation a = (2 * (B - A) - v * T) / T ^ 2

            // DO I WANT THE ACCELERATION MAGNITUDE OR DO I WANT A VECTOR???
            // It depends-- if we change this back to magnitude we can multiply by a vector in the direction of the target to rescale

            // float accelerationMagnitude = (2 * (distanceToFinalBallPosition - distanceToTarget) - initialVelocityMagnitude * timeToReachTarget) / (timeToReachTarget * timeToReachTarget);
            // float accelerationMagnitude = (2 * (finalBallPosition - transform.position) - (initialVelocity * timeToReachTarget)) / (timeToReachTarget * timeToReachTarget);
            // float accelerationMagnitude = (2 * (target.position - transform.position - initialVelocity * timeToReachTarget)).magnitude / (timeToReachTarget * timeToReachTarget);

            // Calculate the acceleration using (finalposition) = initialposition + initialvelocity*time + (acceleration*time^2)/2 rearranged for acceleration
            accelerationForceVector = (2 * (target.position - transform.position - initialVelocity * timeToReachTarget)) / (timeToReachTarget * timeToReachTarget);

        }

        else
        {
            Debug.LogWarning("Try aiming at a target!");
            accelerationForceVector = Vector3.zero;
            DisableAcceleration();
            return;
        }
    }

    private void ApplyAcceleration()
    {
        if (accelerationForceVector != Vector3.zero)
        {
            Debug.Log("Applying acceleration: " + accelerationForceVector);
        }
        else
        {
            Debug.Log("Acceleration is zero");
            DisableAcceleration();
            return; // Should I return here?
        }

        // Forcemode acceleration verses forcemode force?
        ballRigidbody.AddForce(accelerationForceVector * scaleFactor, ForceMode.Acceleration);
        /*
        // Scale the force by fixedDeltaTime outside of the AddForce call
        Vector3 force = accelerationForceVector * scaleFactor * Time.fixedDeltaTime;

        // Apply the force in the direction of the target
        Vector3 direction = (target.position - transform.position).normalized;
        ballRigidbody.AddForce(force.magnitude * direction, ForceMode.Force);
        */
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHands"))
        {
            Debug.Log("Exited PlayerHands");
            isAccelerating = true;
            Debug.Log("isAccelerating set to true");
            initialVelocity = ballRigidbody.velocity;
            Debug.Log("Ball velocity: " + ballRigidbody.velocity);
            CalculateAdjustmentAcceleration();
        }
    }

    // Called in other scripts and methods to disable the acceleration force when the ball is respawned, caught, etc.
    public void DisableAcceleration()
    {
        isAccelerating = false;
    }

    private void FixedUpdate()
    {
        if (isAccelerating)
        {
            //float distanceToTarget = (transform.position - target.position).sqrMagnitude;
            float distanceToPlayer = (transform.position - player.position).sqrMagnitude;

            //if (distanceToPlayer / 4f > distanceToTarget)
            if (distanceToPlayer > maxAcceleratingDistance)
            {
                Debug.Log("Disabling acceleration");
                DisableAcceleration();
            }
            else
            {
                ApplyAcceleration();
            }
        }
    }

}






























/* LAST ATTEMPT PRE SLIDER TRAJECTORY ADJUSTMENT





using UnityEngine;

public class BallTrajectoryAdjustmentManual : MonoBehaviour
{
    [SerializeField] public Transform target;
    private bool isAccelerating = false; // the artist formerly known as applyCurve
    [SerializeField] private Transform player;
    private Vector3 initialVelocity;
    private Vector3 accelerationForceVector; // the artist formerly known as curveAcceleration
    private Rigidbody ballRigidbody;
    [SerializeField] private float scaleFactor = 1f;
    [SerializeField] private float curveMax = .1f;

    private void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void CalculateAdjustmentAcceleration()
    {
        // Check the angle difference between the throw and the target to make sure it falls within a 40 degree threshold
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 initialDirection = initialVelocity.normalized;
        float angle = Vector3.Angle(initialDirection, directionToTarget);

        Debug.Log("Angle: " + angle);

        if (angle <= 40f)
        {

            // Calculate the distance between the player and the target
            Debug.Log("Angle within range");

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            float initialVelocityMagnitude = initialVelocity.magnitude;

            // If throw is invalid, return
            if (initialVelocityMagnitude == 0f)
            {
                Debug.LogWarning("Initial velocity is zero, acceleration calculation skipped.");
                accelerationForceVector = Vector3.zero;
                DisableAcceleration();
                return;
            }

            

            // Calculate the amount of time it would take for the ball to travel that distance given its initial velocity
            float timeToReachTarget = distanceToTarget / initialVelocityMagnitude;
            Vector3 finalBallPosition = transform.position + initialVelocity * timeToReachTarget;
            float distanceToFinalBallPosition = Vector3.Distance(finalBallPosition, target.position);

            if (timeToReachTarget == 0f)
            {
                Debug.LogWarning("Time to reach target is zero, acceleration calculation skipped.");
                accelerationForceVector = Vector3.zero;
                DisableAcceleration();
                return;
            }

            // Calculate the distance between the final ball position at that time and the target
            // Calculate a constant acceleration to be applied to the ball to get it to end up at the target's location by 
            // altering the trajectory of the throw over the course of its flight path using the equation a = (2 * (B - A) - v * T) / T ^ 2

            // DO I WANT THE ACCELERATION MAGNITUDE OR DO I WANT A VECTOR???
            // It depends-- if we change this back to magnitude we can multiply by a vector in the direction of the target to rescale

            // float accelerationMagnitude = (2 * (distanceToFinalBallPosition - distanceToTarget) - initialVelocityMagnitude * timeToReachTarget) / (timeToReachTarget * timeToReachTarget);
            // float accelerationMagnitude = (2 * (finalBallPosition - transform.position) - (initialVelocity * timeToReachTarget)) / (timeToReachTarget * timeToReachTarget);
            // float accelerationMagnitude = (2 * (target.position - transform.position - initialVelocity * timeToReachTarget)).magnitude / (timeToReachTarget * timeToReachTarget);

            // Calculate the acceleration using (finalposition) = initialposition + initialvelocity*time + (acceleration*time^2)/2 rearranged for acceleration
            accelerationForceVector = (2 * (target.position - transform.position - initialVelocity * timeToReachTarget)) / (timeToReachTarget * timeToReachTarget);

        }

        else
        {
            Debug.LogWarning("Try aiming at a target!");
            accelerationForceVector = Vector3.zero;
            DisableAcceleration();
            return;
        }
    }

    private void ApplyAcceleration()
    {
        if (accelerationForceVector != Vector3.zero)
        {
            Debug.Log("Applying acceleration: " + accelerationForceVector);
        }
        else
        {
            Debug.Log("Acceleration is zero");
            DisableAcceleration();
            return; // Should I return here?
        }

        ballRigidbody.AddForce(accelerationForceVector * scaleFactor * Time.deltaTime, ForceMode.Acceleration);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHands"))
        {
            Debug.Log("Exited PlayerHands");
            isAccelerating = true;
            Debug.Log("isAccelerating set to true");
            initialVelocity = ballRigidbody.velocity;
            Debug.Log("Ball velocity: " + ballRigidbody.velocity);
            CalculateAdjustmentAcceleration();
        }
    }

    // Called in other scripts and methods to disable the acceleration force when the ball is respawned, caught, etc.
    public void DisableAcceleration()
    {
        isAccelerating = false;
    }

    private void Update()
    {
        if (isAccelerating)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > distanceToTarget)
            {
                Debug.Log("Disabling acceleration");
                DisableAcceleration();
            }
            else
            {
                ApplyAcceleration();
            }
        }
    }

}
*/
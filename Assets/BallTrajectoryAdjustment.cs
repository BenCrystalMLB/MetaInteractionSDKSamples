using UnityEngine;

public class BallTrajectoryAdjustment : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] [Range(0.0f, 1.0f)] public float adjustmentPercentage = 0.5f;

    private Rigidbody ballRigidbody;

    private void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    public void AdjustTrajectory()
    {
        Debug.Log("Adjusting trajectory");

        // Calculate the direction vector from the ball to the target
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // Calculate the desired velocity to reach the target
        Vector3 desiredVelocity = directionToTarget * ballRigidbody.velocity.magnitude;

        // Interpolate between the current velocity and the desired velocity based on the adjustment percentage
        Vector3 adjustedVelocity = Vector3.Lerp(ballRigidbody.velocity, desiredVelocity, adjustmentPercentage);

        // Apply the adjusted velocity to the ball's Rigidbody
        ballRigidbody.velocity = adjustedVelocity;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHands"))
        {
            Debug.Log("Trigger exit detected with PlayerHands");
            AdjustTrajectory();
        }
    }
    /*
    void OnCollisionExit(Collision collision)
    {
        // Check if the object the ball is no longer in contact with is the player's hand
        if (collision.gameObject.CompareTag("PlayerHands"))
        {
            // Call your AdjustTrajectory method here
            Debug.Log("Collision exit detected");
            AdjustTrajectory();
        }
    }
    */
}

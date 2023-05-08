using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Custom gravity in the "ball" script to give more headroom for catch testing
 */

public class BallLauncher : MonoBehaviour
{
    public GameObject ballObject;
    public Transform player;
    public float launchInterval = 5f;
    public float launchSpeed = 10f;
    public bool launchEnabled = true;
    public Vector3 _customGravity = new Vector3(0, -9.81f, 0);

    public float launchAngle = 45f;

    private float timer;

    private void Start()
    {
        timer = launchInterval;
    }

    private void FixedUpdate()
    {
        if (launchEnabled)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                LaunchBall();
                timer = launchInterval;
            }
        }
    }

    // Uses user-defined launch speed
    /*
    private void LaunchBall()
    {
        // Reset the ball's position and velocity
        ballObject.transform.position = transform.position;
        Rigidbody ballRigidbody = ballObject.GetComponent<Rigidbody>();
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        // Calculate launch velocity
        Vector3 targetDirection = (player.position - transform.position).normalized;
        Vector3 launchVelocity = CalculateLaunchVelocity(targetDirection, launchSpeed);
        ballRigidbody.velocity = launchVelocity;
    }
    */

    // Automatically calculates minimum launch velocity
    /*
    private void LaunchBall()
    {
        // Reset the ball's position and velocity
        ballObject.transform.position = transform.position;
        Rigidbody ballRigidbody = ballObject.GetComponent<Rigidbody>();
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        // Calculate the minimum launch speed required to reach the player
        float minLaunchSpeed = CalculateMinimumLaunchSpeed(player.position);
        launchSpeed = Mathf.Max(launchSpeed, minLaunchSpeed);

        // Calculate launch velocity
        Vector3 targetDirection = (player.position - transform.position).normalized;
        Vector3 launchVelocity = CalculateLaunchVelocity(targetDirection, launchSpeed);
        ballRigidbody.velocity = launchVelocity;
    }
    */
    private void LaunchBall()
    {
        // Reset the ball's position and velocity
        ballObject.transform.position = transform.position;
        Rigidbody ballRigidbody = ballObject.GetComponent<Rigidbody>();
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        // Calculate launch velocity
        Vector3 targetDirection = ((player.position - new Vector3(0,.4f,0)) - transform.position).normalized;
        float adjustedLaunchSpeed = CalculateAdjustedLaunchSpeed(player.position, launchAngle);
        Vector3 launchVelocity = CalculateLaunchVelocity(targetDirection, adjustedLaunchSpeed, launchAngle);
        ballRigidbody.velocity = launchVelocity;
    }
    private float CalculateAdjustedLaunchSpeed(Vector3 targetPosition, float angle)
    {
        float distance = (new Vector3(targetPosition.x, 0, targetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z)).sqrMagnitude;
        float heightDifference = targetPosition.y - transform.position.y;
        float gravity = ballObject.GetComponent<Ball>().GetCustomGravity().magnitude;
        float radians = angle * Mathf.Deg2Rad;

        // Calculate the required launch speed using the ballistic trajectory equation
        float speed = Mathf.Sqrt((gravity * distance * distance) / (2 * distance * Mathf.Tan(radians) - 2 * heightDifference));

        // Check if the calculated speed is a valid number
        if (float.IsNaN(speed))
        {
            Debug.LogWarning("Invalid launch speed. Adjust the launch angle or distance to the player.");
            return launchSpeed;
        }

        return speed;
    }


    /*
    private void LaunchBall()
    {
        // Reset the ball's position and velocity
        ballObject.transform.position = transform.position;
        Rigidbody ballRigidbody = ballObject.GetComponent<Rigidbody>();
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        // Calculate the minimum launch speed required to reach the player
        float minLaunchSpeed = CalculateMinimumLaunchSpeed(player.position);

        // Set the launch speed to the maximum of the specified launch speed and the calculated minimum launch speed
        float adjustedLaunchSpeed = Mathf.Min(launchSpeed, minLaunchSpeed);

        // Calculate launch velocity
        Vector3 targetDirection = (player.position - transform.position).normalized;
        Vector3 launchVelocity = CalculateLaunchVelocity(targetDirection, adjustedLaunchSpeed);
        ballRigidbody.velocity = launchVelocity;
    }*/

    private float CalculateMinimumLaunchSpeed(Vector3 targetPosition)
    {
        float distance = (new Vector3(targetPosition.x, 0, targetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z)).sqrMagnitude;
        float heightDifference = targetPosition.y - transform.position.y;
        float gravity = ballObject.GetComponent<Ball>().GetCustomGravity().magnitude;

        // Determine the optimal launch angle using the quadratic formula
        float a = gravity * distance * distance;
        float b = -2 * heightDifference * distance;
        float c = 0;
        float discriminant = b * b - 4 * a * c;

        // Check if the discriminant is negative, indicating that there is no valid launch angle
        if (discriminant < 0)
        {
            Debug.LogWarning("Invalid launch angle: No solution for the specified launch parameters.");
            return launchSpeed;
        }

        // Calculate the optimal launch angle
        float angle = Mathf.Atan((-b + Mathf.Sqrt(discriminant)) / (2 * a));

        // Calculate the minimum launch speed using the optimal launch angle
        float speed = Mathf.Sqrt((gravity * distance) / (2 * Mathf.Cos(angle) * Mathf.Sin(angle)));

        return speed;
    }


    private Vector3 CalculateLaunchVelocity(Vector3 direction, float speed, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        Vector3 launchDirection = horizontalDirection * Mathf.Cos(radians) + Vector3.up * Mathf.Sin(radians);
        return launchDirection * speed;
    }


    private Vector3 CalculateLaunchVelocity(Vector3 direction, float speed)
    {
        float distance = (player.position - transform.position).sqrMagnitude;
        float gravity = ballObject.GetComponent<Ball>().GetCustomGravity().magnitude;
        //float gravity = ballObject.GetComponent<Ball>().customGravity.magnitude; // Physics.gravity.magnitude;

        // Calculate the required launch angle using the ballistic trajectory equation
        float angle = 0.5f * Mathf.Asin((gravity * distance) / (speed * speed));

        // Check if the calculated angle is a valid number
        if (float.IsNaN(angle))
        {
            Debug.LogWarning("Invalid launch angle. Adjust the launch speed or distance to the player.");
            return Vector3.zero;
        }

        // Calculate the launch direction
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        Vector3 launchDirection = horizontalDirection * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);

        return launchDirection * speed;
    }

    /*
    private Vector3 CalculateLaunchVelocity(Vector3 direction, float speed, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        Vector3 launchDirection = horizontalDirection * Mathf.Cos(radians) + Vector3.up * Mathf.Sin(radians);
        return launchDirection * speed;
    }*/

    public void ToggleLaunchEnabled()
    {
        launchEnabled = !launchEnabled;
    }

    public void SetLaunchInterval(float newInterval)
    {
        launchInterval = newInterval;
    }

    public void SetLaunchSpeed(float newSpeed)
    {
        launchSpeed = newSpeed;
    }

    public Vector3 CustomGravity
    {
        get => _customGravity;
        set
        {
            _customGravity = value;
            Ball ballComponent = ballObject.GetComponent<Ball>();
            if (ballComponent != null)
            {
                ballComponent.SetCustomGravity(_customGravity);
            }
        }
    }

    




}

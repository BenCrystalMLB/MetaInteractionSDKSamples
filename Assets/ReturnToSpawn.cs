using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToSpawn : MonoBehaviour
{
    public Vector3 initialPosition;
    public int updateInterval = 1;
    public float minY = 0;

    private Rigidbody rb;
    private BallTrajectoryAdjustmentManual ballTrajectoryAdjustmentManual;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = this.transform.position;

        rb = GetComponent<Rigidbody>();
        ballTrajectoryAdjustmentManual = GetComponent<BallTrajectoryAdjustmentManual>();

        StartCoroutine(CheckPositionAndReset());
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(CheckPositionAndReset());
    }

    private IEnumerator CheckPositionAndReset()
    {
        while (true)
        {
            if (transform.position.y < minY)
            {
                transform.position = initialPosition;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Disable trajectory adjustment and acceleration
                ballTrajectoryAdjustmentManual.DisableAcceleration();
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
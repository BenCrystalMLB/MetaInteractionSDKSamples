using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Vector3 customGravity = new Vector3(0, -9.81f, 0);

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ApplyCustomGravity();
    }

    private void ApplyCustomGravity()
    {
        rb.AddForce(customGravity, ForceMode.Acceleration);
    }

    public Vector3 GetCustomGravity()
    {
        return customGravity;
    }

    public void SetCustomGravity(Vector3 newGravity)
    {
        customGravity = newGravity;
    }



}

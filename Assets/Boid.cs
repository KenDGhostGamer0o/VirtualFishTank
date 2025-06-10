using UnityEngine;

public class Boid : MonoBehaviour
{
    public Rigidbody rigidbody;

    public float speedMax = 2;
    public float accelMax = 3;
    internal readonly float acceleration;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, rigidbody.linearVelocity, Color.red);
    }

    public Vector3 Seek(Vector3 target, float acceleration)
    {
        Vector3 toTarget = target - transform.position;

        Vector3 toTargetNormalized = toTarget.normalized;

        Vector3 accel = toTargetNormalized * acceleration;

        return accel;
    }

    public Vector3 Pursue(Vector3 target, float acceleration, float desiredSpeed)
    {
        Vector3 toTarget = target - transform.position;

        Vector3 toTargetNormalized = toTarget.normalized;

        Vector3 desiredVelocity = toTargetNormalized * desiredSpeed;

        Vector3 deltaVel = desiredVelocity - rigidbody.linearVelocity;

        Vector3 accel = toTargetNormalized * acceleration;

        return accel;
    }

    private void FixedUpdate()
    {
        float speed = rigidbody.linearVelocity.magnitude;

        if (speed > speedMax)
        {
            rigidbody.linearVelocity = rigidbody.linearVelocity * speedMax / speed;
        }
    }
    private void Awake()
    {
        GetComponent<Renderer>().material.SetColor(" BaseColor ", Random.ColorHSV(0, 1, 0f, 1f, 0.5f, 1f));
    }

    internal Vector3 Seek(Vector3 position, Boid boid)
    {
        throw new System.NotImplementedException();
    }
}

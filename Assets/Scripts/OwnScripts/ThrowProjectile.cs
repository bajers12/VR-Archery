using UnityEngine;

[RequireComponent(typeof(Transform))]
public class BezierProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    private Vector3 p0, p1, p2;
    private float duration;
    private float elapsed;
    public float spinSpeed = 1800f;
    public float rotationSpeed = 720f;

    // Hit logic
    [Header("Hit Logic")]
    private EnemyTarget target;
    private int damage;





    public void Launch(Vector3 start, EnemyTarget target, float flightTime, float arcHeight, int damage)
    {
        this.target   = target;
        this.damage   = damage;

        p0 = start;
        p2 = target.transform.position;
        p1 = (p0 + p2) * 0.5f + Vector3.up * arcHeight;

        duration = Mathf.Max(0.01f, flightTime);
        elapsed  = 0f;

        transform.position = p0;
        // Face initial direction
        Vector3 initDir = (p1 - p0);
        if (initDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(initDir.normalized);
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);
        float u = 1 - t;

        // Bézier position
        Vector3 pos = u*u*p0 + 2*u*t*p1 + t*t*p2;
        transform.position = pos;

        // Compute travel direction tangent
        Vector3 tangent = 2*u*(p1 - p0) + 2*t*(p2 - p1);

        if (t < 1f)
        {
            if (tangent.sqrMagnitude > 0.001f)
            {
                // Smoothly rotate to face travel direction
                Quaternion desiredRot = Quaternion.LookRotation(tangent.normalized);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    desiredRot,
                    Time.deltaTime * rotationSpeed
                );

                transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
            }
        }
        else
        {
            // Hit target
            if (target != null)
                target.GetHit(damage);
            Destroy(gameObject);
        }
    }
}

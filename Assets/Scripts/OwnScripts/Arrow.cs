using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Launch Settings")]
    public float speed = 10f;

    [Header("Gravity Settings")]
    [Tooltip("1 = normal gravity, <1 = floatier, >1 = heavier")]
    public float gravityScale = 0.5f;

    [Header("References")]
    public Transform tip;

    private Rigidbody _rb;
    private bool _inAir = false;
    private Vector3 _lastPosition;

    // Exclude layers 8 & 9 from collision
    private int collisionMask = ~((1 << 8) | (1 << 9));

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // Start parked with no physics
        _rb.isKinematic = true;
        _rb.useGravity = false;

        PullInteraction.PullActionReleased += Release;
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= Release;
    }

    private void Release(float pullValue)
    {
        PullInteraction.PullActionReleased -= Release;
        transform.parent = null;
        _inAir = true;

        // Enable physics but disable built-in gravity
        _rb.isKinematic = false;
        _rb.useGravity  = false;

        // Apply initial impulse
        Vector3 force = transform.forward * speed * pullValue;
        _rb.AddForce(force, ForceMode.Impulse);

        // Start flight/rotation
        StartCoroutine(RotateWithVelocity());
        _lastPosition = tip.position;
    }

    private IEnumerator RotateWithVelocity()
    {
        // Wait one physics frame
        yield return new WaitForFixedUpdate();

        while (_inAir)
        {
            // Apply custom gravity (downwards acceleration)
            _rb.AddForce(Physics.gravity * gravityScale,
                         ForceMode.Acceleration);

            // Rotate arrow to match velocity direction
            transform.rotation = Quaternion.LookRotation(_rb.velocity,
                                                        transform.up);

            // Collision check
            if (Physics.Linecast(_lastPosition, tip.position,
                                 out RaycastHit hit, collisionMask))
            {
                if (hit.transform.TryGetComponent<Rigidbody>(out Rigidbody body))
                {
                    // Stick the arrow and transfer impulse
                    _rb.interpolation = RigidbodyInterpolation.None;
                    transform.parent = hit.transform;
                    body.AddForce(_rb.velocity, ForceMode.Impulse);
                }
                if (hit.transform.TryGetComponent<Target>(out Target target))
                {
                    GameManager.Instance.AddScore(target.scoreValue);
                }
                StopArrow();
                yield break;
            }

            _lastPosition = tip.position;
            yield return new WaitForFixedUpdate();
        }
    }

    private void FixedUpdate()
    {
        // (Optional: if you need other physics logic outside the coroutine)
    }

    private void StopArrow()
    {
        _inAir = false;
        _rb.isKinematic = true;
        _rb.useGravity  = false;
    }
}

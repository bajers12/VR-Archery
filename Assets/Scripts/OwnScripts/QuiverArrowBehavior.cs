using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable))]
public class QuiverArrowBehavior : MonoBehaviour
{
    Rigidbody          _rb;
    XRGrabInteractable _grab;

    void Awake()
    {
        _rb   = GetComponent<Rigidbody>();
        _grab = GetComponent<XRGrabInteractable>();

        // 1) Start locked in the quiver
        _rb.isKinematic = true;
        _rb.useGravity  = false;

        // 2) Listen for release
        _grab.selectExited.AddListener(OnReleased);
    }

    void OnDestroy()
    {
        // Clean up listener
        _grab.selectExited.RemoveListener(OnReleased);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        // 3) When it’s let go, turn physics back on
        _rb.isKinematic = false;
        _rb.useGravity  = true;
        transform.SetParent(null, true);
    }
}

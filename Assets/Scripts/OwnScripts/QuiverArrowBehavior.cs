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

        // 1) Start locked in place
        _rb.isKinematic = true;
        _rb.useGravity  = false;

        // 2) Listen for “grab” event
        _grab.selectEntered.AddListener(OnGrabbed);
    }

    void OnDestroy()
    {
        // Clean up listener
        _grab.selectEntered.RemoveListener(OnGrabbed);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        // 3) When the player grabs it, let physics take over
        _rb.isKinematic = false;
        _rb.useGravity  = true;
    }
}

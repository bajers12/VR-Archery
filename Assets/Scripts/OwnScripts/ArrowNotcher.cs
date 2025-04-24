using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowNotcher : MonoBehaviour
{
    [Header("Swap Prefabs")]
    [Tooltip("Prefab to instantiate when a quiver arrow is notched")]
    public GameObject bowArrowPrefab;

    [Header("Notch Point")]
    [Tooltip("Transform where the BowArrow should sit when notched")]
    public Transform notchPoint;
    [SerializeField] private PullInteraction pullInteraction;
    private GameObject _currentBowArrow;
    public Vector3 nockOffset;

    private void Awake()
    {
        PullInteraction.PullActionReleased += OnArrowFired;
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= OnArrowFired;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only notch if we have no arrow currently and the collider is a quiver arrow
        if (_currentBowArrow != null) return;
        if (!other.CompareTag("QuiverArrow")) return;

        // Ensure this arrow is currently grabbed by the user
        var grabInteractable = other.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null || !grabInteractable.isSelected)
            return;
        
        Debug.Log("[BowNotcher] Grabbed QuiverArrow detected in notch.");

        // Destroy the quiver arrow after confirming it's grabbed
        Destroy(other.gameObject);


        _currentBowArrow = Instantiate(
        bowArrowPrefab,
        notchPoint,          // the parent transform
        worldPositionStays: false
        );

        _currentBowArrow.transform.localRotation = Quaternion.identity;


        var arrowScript = _currentBowArrow.GetComponent<Arrow>();
        Vector3 nockLocal = arrowScript.nockPoint.localPosition;

        _currentBowArrow.transform.localPosition = -nockLocal;
        pullInteraction.SetNocked(true);

    }

    private void OnArrowFired(float pullAmount)
    {
        if (_currentBowArrow == null) return;
        Debug.Log("[BowNotcher] Arrow fired! Clearing notch.");
        _currentBowArrow = null;
    }
}
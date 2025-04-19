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

    private GameObject _currentBowArrow;

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

        // Instantiate the bow arrow at the notch point
        _currentBowArrow = Instantiate(
            bowArrowPrefab,
            notchPoint.position,
            notchPoint.rotation
        );

        // Parent it under the notch but keep its world transform
        _currentBowArrow.transform.SetParent(notchPoint, true);

        Debug.Log("[BowNotcher] BowArrow instantiated and notched.");
    }

    private void OnArrowFired(float pullAmount)
    {
        if (_currentBowArrow == null) return;
        Debug.Log("[BowNotcher] Arrow fired! Clearing notch.");
        _currentBowArrow = null;
    }
}
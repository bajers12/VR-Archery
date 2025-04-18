using UnityEngine;

public class BowNotcher : MonoBehaviour
{
    [Header("Swap Prefabs")]
    [Tooltip("Prefab to instantiate when a quiver arrow is notched")]
    public GameObject bowArrowPrefab;

    [Header("Notch Point")]
    [Tooltip("Transform where the BowArrow should sit when notched")]
    public Transform notchPoint;

    GameObject _currentBowArrow;

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
        if (_currentBowArrow != null) return;
        if (!other.CompareTag("QuiverArrow")) return;

        Debug.Log("[BowNotcher] QuiverArrow detected in notch.");

        // destroy the quiver arrow
        Destroy(other.gameObject);

        // 1) Instantiate at world‐space notch position/rotation
        _currentBowArrow = Instantiate(
            bowArrowPrefab,
            notchPoint.position,
            notchPoint.rotation
        );

        // 2) Parent it under the notch but KEEP its world transform (incl. scale)
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

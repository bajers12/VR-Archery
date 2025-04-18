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
        // Listen for the moment the arrow is released
        PullInteraction.PullActionReleased += OnArrowFired;
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= OnArrowFired;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentBowArrow != null) return;

        // only react to QuiverArrow tags
        if (!other.CompareTag("QuiverArrow"))
            return;

        Debug.Log("[BowNotcher] QuiverArrow detected in notch.");

        // 1) Destroy the quiver arrow you just slid in
        Destroy(other.gameObject);

        // 2) Spawn the BowArrow and parent it to the notch
        _currentBowArrow = Instantiate(
            bowArrowPrefab,
            notchPoint.position,
            notchPoint.rotation,
            notchPoint
        );
        _currentBowArrow.transform.localPosition = Vector3.zero;
        _currentBowArrow.transform.localRotation = Quaternion.identity;

        Debug.Log("[BowNotcher] BowArrow instantiated and notched.");
    }

    private void OnArrowFired(float pullAmount)
    {
        if (_currentBowArrow == null) return;

        Debug.Log("[BowNotcher] Arrow fired! Clearing notch.");
        _currentBowArrow = null;
    }
}

using UnityEngine;

public class EnforceChildLayer : MonoBehaviour
{
    [Tooltip("Layer to enforce on all children (and grandchildren) of this object.")]
    public int targetLayer = 8;

    // This method is called by Unity whenever the child hierarchy changes.
    void OnTransformChildrenChanged()
    {
        SetLayerRecursively(transform, targetLayer);
    }

    void SetLayerRecursively(Transform parent, int layer)
    {
        foreach (Transform child in parent)
        {
            // Only change the layer if it's not already set
            if (child.gameObject.layer != layer)
            {
                child.gameObject.layer = layer;
            }
            SetLayerRecursively(child, layer);
        }
    }
}
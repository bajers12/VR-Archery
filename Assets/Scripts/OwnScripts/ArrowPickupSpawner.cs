using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowPickupSpawner : MonoBehaviour
{
    [Header("Arrow Prefab")]
    public GameObject arrowPrefab;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Delay (seconds)")]
    public float spawnDelay = 0.2f;

    GameObject currentArrow;

    void Start()
    {
        SpawnArrow();
    }

        // 1) Instantiate the arrow as a child of this box‑spawner
    void SpawnArrow()
    {
        // 1) Instantiate without a parent, so it comes in at the correct world‐scale:
        currentArrow = Instantiate(
            arrowPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // 2) Now parent it—but tell Unity to keep its world‐space position, rotation AND scale:
        currentArrow.transform.SetParent(transform, true);

        // 3) Hook into its grab event
        var grab = currentArrow.GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnArrowGrabbed);
    }   


    void OnArrowGrabbed(SelectEnterEventArgs args)
    {
        // 1) Unsubscribe so this arrow only spawns once
        var grab = args.interactableObject as XRGrabInteractable;
        grab.selectEntered.RemoveListener(OnArrowGrabbed);

        // 2) Unparent it so it flies free
        currentArrow.transform.parent = null;
        currentArrow = null;

        // 3) Queue the next spawn
        StartCoroutine(DelayedSpawn());
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnArrow();
    }
}

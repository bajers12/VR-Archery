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

    void SpawnArrow()
    {
        // 1) Instantiate the arrow as a child of this box‑spawner
        currentArrow = Instantiate(
            arrowPrefab,
            spawnPoint.position,
            spawnPoint.rotation,
            transform
        );

        // 2) Hook into its grab event so we know when it's picked up
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

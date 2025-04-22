using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
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
        // ensure our collider is a trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
        SpawnArrow();
    }

    void SpawnArrow()
    {
        currentArrow = Instantiate(
            arrowPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
        // keep world‐scale/pos/rot when parented
        currentArrow.transform.SetParent(transform, true);

        var grab = currentArrow.GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnArrowGrabbed);
    }

    void OnArrowGrabbed(SelectEnterEventArgs args)
    {
        SendHapticFeedback(args.interactorObject);
        CleanupAndQueue();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentArrow)
        {
            CleanupAndQueue();
        }
    }

    void CleanupAndQueue()
    {
        // remove listener so we only process one removal
        var grab = currentArrow.GetComponent<XRGrabInteractable>();
        grab.selectEntered.RemoveListener(OnArrowGrabbed);

        // unparent so it can fly/destroy freely
        currentArrow.transform.parent = null;
        currentArrow = null;

        // schedule the next arrow
        StartCoroutine(DelayedSpawn());
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnArrow();
    }

    void SendHapticFeedback(IXRInteractor interactor)
{
    if (interactor is XRBaseControllerInteractor controllerInteractor)
    {
        var controller = controllerInteractor.xrController;
        if (controller != null)
        {
            controller.SendHapticImpulse(0.5f, 0.1f);
        }
    }
}

}

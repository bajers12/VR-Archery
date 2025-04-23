using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullInteraction : XRBaseInteractable
{
    public static event Action<float> PullActionReleased;

    [Tooltip("How far (0–1) you have to pull before the arrow actually fires")]
    [Range(0f, 1f)]
    public float minPullThreshold = 0.35f;


    [Header("Sound")]
    public AudioSource crackleSource;
    public AudioSource releaseSource;
    public Transform start, end;
    public GameObject notch;

    public float pullAmount { get; private set; } = 0f;

    private LineRenderer _lineRenderer;
    private IXRSelectInteractor pullingInteractor = null;

    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        pullingInteractor = args.interactorObject;
        if (crackleSource && !crackleSource.isPlaying)
            crackleSource.Play();
    }

    public void Release()
    {
        // only invoke the release event if we've passed the threshold
        if (pullAmount >= minPullThreshold)
        {
            SendHapticFeedback(pullingInteractor);
            PullActionReleased?.Invoke(pullAmount);
            if (releaseSource){
                releaseSource.volume = pullAmount;
                releaseSource.pitch = Mathf.Lerp(0.9f, 1.3f, pullAmount);
                releaseSource.PlayOneShot(releaseSource.clip);
                }
        }
        if (crackleSource && crackleSource.isPlaying)
            crackleSource.Stop();

        // reset everything whether or not we shot
        pullingInteractor = null;
        pullAmount = 0f;
        notch.transform.localPosition = new Vector3(
            notch.transform.localPosition.x,
            notch.transform.localPosition.y,
            0f
        );
        UpdateString();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected)
        {
            Vector3 pullPosition = pullingInteractor.transform.position;
            pullAmount = CalculatePull(pullPosition);
            UpdateString();

            if (crackleSource){
                crackleSource.volume = pullAmount; // or curve it: Mathf.SmoothStep(0f, 1f, pullAmount)
                }
        }
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        Vector3 direction = end.position - start.position;
        float maxLen = direction.magnitude;
        float currentLen = Vector3.Project(pullPosition - start.position, direction.normalized).magnitude;
        return Mathf.Clamp(currentLen / maxLen, 0f, 1f);
    }

    private void UpdateString()
    {
        float z = Mathf.Lerp(start.localPosition.z, end.localPosition.z, pullAmount);
        notch.transform.localPosition = new Vector3(
            notch.transform.localPosition.x,
            notch.transform.localPosition.y,
            z + 0.2f
        );
        _lineRenderer.SetPosition(1, Vector3.forward * z);
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

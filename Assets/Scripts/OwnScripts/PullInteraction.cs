using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullInteraction : XRBaseInteractable
{
    public static event Action<float> PullActionReleased;

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
    }

    public void Release()
    {
        PullActionReleased?.Invoke(pullAmount);
        pullingInteractor = null;
        pullAmount = 0f;
        notch.transform.localPosition = new UnityEngine.Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0f);
        UpdateString();

    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic){
            if (isSelected){
                UnityEngine.Vector3 pullPosition = pullingInteractor.transform.position;
                pullAmount = CalculatePull(pullPosition);

                UpdateString();
            }
        }
    }

    private float CalculatePull(UnityEngine.Vector3 pullPosition)
    {
        UnityEngine.Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;

        float currentLength = UnityEngine.Vector3.Project(pullPosition - start.position, targetDirection.normalized).magnitude;

        return Mathf.Clamp(currentLength / maxLength, 0f, 1f);
    }

    
    private void UpdateString()
    {
        UnityEngine.Vector3 linePosition = UnityEngine.Vector3.forward * Mathf.Lerp(start.transform.localPosition.z,
                                                                             end.transform.localPosition.z, pullAmount);
        notch.transform.localPosition = new UnityEngine.Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosition.z +.2f);
        _lineRenderer.SetPosition(1, linePosition);

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

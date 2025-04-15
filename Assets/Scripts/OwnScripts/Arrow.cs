using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;

    private Rigidbody _rb;
    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        PullInteraction.PullActionReleased += Release;

        Stop();
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= Release;
    }

    private void Release(float value){
        PullInteraction.PullActionReleased -= Release;
        gameObject.transform.parent = null;
        _inAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward * speed * value;
        _rb.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());

        _lastPosition = tip.position;

    }

    IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir){
            Quaternion newRotation = Quaternion.LookRotation(_rb.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    void FixedUpdate()
    {
        if (_inAir){
            CheckCollision();
            _lastPosition = tip.position;
        }
    }


    private void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition, tip.position, out RaycastHit hitInfo)){
            if (hitInfo.transform.gameObject.layer != 8 && hitInfo.transform.gameObject.layer != 9){
                if (hitInfo.transform.gameObject.TryGetComponent(out Rigidbody body)){

                    _rb.interpolation = RigidbodyInterpolation.None;
                    transform.parent = hitInfo.transform;
                    body.AddForce(_rb.velocity, ForceMode.Impulse);
                }
                Stop();
            }
        }
    }
    private void Stop(){
        _inAir = false;
        SetPhysics(false);
    }

    private void SetPhysics(bool usePhysics){
        _rb.isKinematic = !usePhysics;
        _rb.useGravity = usePhysics;
    }
    
}

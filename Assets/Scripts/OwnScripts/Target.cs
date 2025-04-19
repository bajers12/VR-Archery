using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int scoreValue = 100; // The score value for this target

    public bool destroyOnHit = false; // Whether to destroy the target on hit

    public void hit()
    {

        // Optionally destroy the target
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }
}

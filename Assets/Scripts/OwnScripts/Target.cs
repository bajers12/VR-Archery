
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Target : MonoBehaviour
{
    public int scoreValue = 100;
    public bool destroyOnHit = false;

    public void hit()
    {
        
        Transform t = transform;
        while (!t.CompareTag("Enemy"))
        {
            t = t.parent;
        }
        if (!destroyOnHit){
            return;
        }


        // If we found an Enemy up the tree, destroy it; otherwise destroy this
        if (t.CompareTag("Enemy"))
            Destroy(t.gameObject);
        else
            Destroy(gameObject);
    }
}

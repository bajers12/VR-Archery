
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Target : MonoBehaviour
{
    public int scoreValue = 100;
    public bool destroyOnHit = false;
    public ParticleSystem deathEffectPrefab;

    public void hit()
    {
        
        Transform t = transform;
        if (t.CompareTag("Enemy"))
        Destroy(t.gameObject);
        while (!t.CompareTag("Enemy"))
        {
            t = t.parent;

        }
        if (!destroyOnHit){
            return;
        }
        t.GetComponent<DeathEffect>().die();
        if (t.CompareTag("Enemy"))
            Destroy(t.gameObject);
        else
            Destroy(gameObject);
            
    }
    
    private void OnDestroy()
    {
    }

}

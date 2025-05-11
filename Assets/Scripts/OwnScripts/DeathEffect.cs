using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [Header("VFX")]
    public ParticleSystem deathEffectPrefab;

    public void die()
    {
        if (deathEffectPrefab != null)
        {
            var fx = Instantiate(
                deathEffectPrefab,
                transform.position,
                Quaternion.LookRotation(Vector3.up)
            );
            Destroy(
                fx.gameObject,
                fx.main.duration + fx.main.startLifetime.constantMax
            );
        }
        Destroy(gameObject);
    }
}

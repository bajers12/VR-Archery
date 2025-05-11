using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTarget : MonoBehaviour
{
    public static EnemyTarget enemyTarget;
    public TextMeshProUGUI gameOverText;

    public float gameResetDelay = 5.0f;
    public int maxLife;
    public int remainingLife;
    public float defenseBufferTime = 3;
    public int defenseBufferThreshhold = 3;
    public float defenseRadius = 5;

    private float timeSinceLastDamageTaken = 0f;
    private int damageTakenBuffer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        enemyTarget = this;
        remainingLife = maxLife;
        timeSinceLastDamageTaken = 0f;
        damageTakenBuffer = 0;
    }

    private void Update()
    {
        timeSinceLastDamageTaken += Time.deltaTime;
    }

    public static EnemyTarget GetTarget()
    {
        return enemyTarget;
    }

    public static int GetCurrentLife()
    {
        return enemyTarget.remainingLife;
    }

    public void GetHit(int damage)
    {
        if(remainingLife > 0)
        {
            Debug.Log("Base is hit");
            remainingLife -= damage;
            HandleDefense(damage);
            timeSinceLastDamageTaken = 0f;

        }
        if (remainingLife <= 0)
        {
            OnBaseDestroyed();
        }
    }

    void HandleDefense(int damageTaken)
    {
        if(timeSinceLastDamageTaken > defenseBufferTime)
        {
            damageTakenBuffer = damageTaken;
        } else
        {
            damageTaken += damageTakenBuffer += damageTaken;
        }

        if(damageTakenBuffer >= defenseBufferThreshhold)
        {
            ClearEnemiesInRadius(defenseRadius);
            damageTakenBuffer = 0;
        }
    }

    private void OnBaseDestroyed()
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(resetGame(gameResetDelay));
    }

    private void ClearEnemiesInRadius(float radius)
    {
        foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(radius > Vector3.Distance(transform.position, e.transform.position))
            {
                return;
            }
            var t = e.GetComponent<DeathEffect>();
            t.die();
            Destroy(e);
        }
    }

    private void ClearAllEnemies()
    {
        foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
        {   
            var t = e.GetComponent<DeathEffect>();
            t.die();
            Destroy(e);
        }
    }

    IEnumerator resetGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.ResetScore();
        SceneManager.LoadScene("MenuScene");
    }

}

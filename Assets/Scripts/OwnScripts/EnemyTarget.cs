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
    // Start is called before the first frame update
    void Awake()
    {
        enemyTarget = this;
        remainingLife = maxLife;
    }

    public static EnemyTarget GetTarget()
    {
        return enemyTarget;
    }

    public static int GetCurrentLife()
    {
        return enemyTarget.remainingLife;
    }

    public void GetHit()
    {
        if(remainingLife > 0)
        {
            Debug.Log("Base is hit");
            remainingLife--;
            ClearAllEnemies();
        }
        if(remainingLife <= 0)
        {
            OnBaseDestroyed();
        }
    }

    private void OnBaseDestroyed()
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(resetGame(gameResetDelay));
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

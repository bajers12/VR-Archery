using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    public static EnemyTarget enemyTarget;
    // Start is called before the first frame update
    void Awake()
    {
        enemyTarget = this;
    }

    public static EnemyTarget GetTarget()
    {
        return enemyTarget;
    }

    public void GetHit()
    {
        Debug.Log("Base is hit");
    }

}

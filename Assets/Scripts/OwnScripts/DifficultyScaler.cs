using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyScaler : MonoBehaviour
{
    public static DifficultyScaler instance { get; private set; }
    float timeSinceGamestart = 0;

    [SerializeField]
    int logBase = 2;
    [SerializeField]
    float timeScale = 15;

    // Start is called before the first frame update
    void OnEnable()
    {
        instance = this;
        timeSinceGamestart = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceGamestart += Time.deltaTime;
    }

    public float GetDifficultyScale()
    {
        return Mathf.Max(Mathf.Log(timeSinceGamestart/timeScale, logBase), 1f);
    }
}

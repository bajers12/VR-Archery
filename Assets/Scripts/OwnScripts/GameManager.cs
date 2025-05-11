using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public AudioSource hitSource;
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    public void ResetScore()
    {
        Score = 0;
    }
    public void AddScore(int amount)
    {
        Score += amount;
    }
}

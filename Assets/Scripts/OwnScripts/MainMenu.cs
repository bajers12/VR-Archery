using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject howToPanel; 

    


    public void OnStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnHowToPlay()
    {
        howToPanel.SetActive(true);
    }

    public void OnBackFromHowTo()
    {
        howToPanel.SetActive(false);
    }
}

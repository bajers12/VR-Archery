using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject howToPanel; 
    public GameObject howToPanel2; 

    public GameObject howToButton;
    public GameObject startButton;

    


    public void OnStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnHowToPlay()
    {
        howToPanel.SetActive(true);
        howToButton.SetActive(false);
        startButton.SetActive(false);
        
    }

    public void OnBackFromHowTo()
    {
        howToPanel.SetActive(false);
        howToButton.SetActive(true);
        startButton.SetActive(true);
    }
    public void OnBackFromHowTo2()
    {
        howToPanel2.SetActive(false);
        howToPanel.SetActive(true);
    }
    public void OnNext(){
        howToPanel.SetActive(false);
        howToPanel2.SetActive(true);
    }
    public void OnExitHowTo(){
        howToPanel2.SetActive(false);
        howToButton.SetActive(true);
        startButton.SetActive(true);
    }
}

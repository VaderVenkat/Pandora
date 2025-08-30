using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Behaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text HealthText;

    [SerializeField] private Button WinButton;
    [SerializeField] private Button LossButton;

    private void Start()
    {
        if (WinButton != null)
        {
            WinButton.gameObject.SetActive(false);
            WinButton.onClick.AddListener(RestartGame);
        }

        if (LossButton != null)
        {
            LossButton.gameObject.SetActive(false);
            LossButton.onClick.AddListener(RestartGame);
        }
    }



    public void RestartGame() // the script of the restart the game [main] 
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Time.timeScale = 1f;
        //SceneManager.GetActiveScene().buildIndex
    }



    public void UpdateHealth(int health)
    {
        if (HealthText != null)
        {
            HealthText.text = "Health: " + health;
        }
    }

    public void ShowGameOver() // showing the gameovwer buttion
    {
        if (LossButton != null)
        { 
            LossButton.gameObject.SetActive(true);

            Time.timeScale = 0f;

        }
    }

    public void ShowWin() // show the we won button
    {
        if (WinButton != null)
        {
            WinButton.gameObject.SetActive(true);

            Time.timeScale = 0f;
        }
    }

   
}

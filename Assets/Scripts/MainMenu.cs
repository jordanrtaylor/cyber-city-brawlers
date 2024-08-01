using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Assign these in the Inspector
    public TMP_Text raceButtonText;
    public TMP_Text arenaButtonText;
    public TMP_Text optionsButtonText;
    public TMP_Text exitButtonText;

    public void Start()
    {
        // Set the initial button texts
        raceButtonText.text = "Race";
        arenaButtonText.text = "Arena";
        optionsButtonText.text = "Options";
        exitButtonText.text = "Exit";
    }

    public void OnRaceButtonClicked()
    {
        // Load the main scene
        SceneManager.LoadScene("RaceScene");
    }

    public void OnArenaButtonClicked()
    {
        // Display not ready message
        Debug.Log("Arena mode is not ready yet.");
    }

    public void OnOptionsButtonClicked()
    {
        // Display not ready message
        Debug.Log("Options menu is not ready yet.");
    }

    public void OnExitButtonClicked()
    {
        // Quit the application
        Application.Quit();
        Debug.Log("Application Quit");
    }
}

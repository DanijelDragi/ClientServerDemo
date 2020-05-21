using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    public GameObject startMenu;
    public InputField usernameField;
    public Text timeDisplay;
    public GameObject endScreen;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Debug.Log("Instance already exists! Destroying this object!");
            Destroy(this);
        }
    }

    public void ConnectToServer() {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.Instance.ConnectToServer();
        timeDisplay.gameObject.SetActive(true);
    }

    public void UpdateTime(int remainingTime) {
        timeDisplay.text = remainingTime.ToString();
    }

    public void GameOver(int score) {
        timeDisplay.gameObject.SetActive(false);
        TargetManager.GameOver();
        endScreen.GetComponentInChildren<Text>().text += score.ToString();
        endScreen.SetActive(true);
    }

    public void QuitGame() {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}

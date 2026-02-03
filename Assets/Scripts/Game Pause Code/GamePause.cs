using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    public GameObject PlayerOrVehicle;
    public GameObject GameMenu;
    public GameObject GameMenu2;
    public GameObject GameMenu3;
    public GameObject GameMenu4;
    public MonoBehaviour CameraLookScript; // add your camera script here (like FirstPersonLook)

    bool isPaused = false;

    void Start()
    {
        GameMenu.SetActive(false);
        GameMenu2.SetActive(false);
        GameMenu3.SetActive(false);
        GameMenu4.SetActive(false);

        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        GameMenu.SetActive(true);
        GameMenu2.SetActive(true);
        GameMenu3.SetActive(true);
        GameMenu4.SetActive(true);

        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (CameraLookScript != null)
            CameraLookScript.enabled = false; // stop camera movement
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        GameMenu.SetActive(false);
        GameMenu2.SetActive(false);
        GameMenu3.SetActive(false);
        GameMenu4.SetActive(false);
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (CameraLookScript != null)
            CameraLookScript.enabled = true; // re-enable camera movement
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home Page");
    }
}

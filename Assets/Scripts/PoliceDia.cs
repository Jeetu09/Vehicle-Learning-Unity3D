using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PoliceDia : MonoBehaviour
{
    public TextMeshProUGUI Timer;      // Text display for timer
    public GameObject QNAPanel;        // Panel to show later
    public Button StartBtn;            // UI Start button

    private float timeRemaining = 120f; // 2 minutes
    private bool timerEnded = false;

    void Start()
    {
        QNAPanel.SetActive(false);
        StartBtn.interactable = false; // disable button initially
    }

    void Update()
    {
        if (!timerEnded)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);
                Timer.text = $"{minutes:00}:{seconds:00}";
            }
            else
            {
                timerEnded = true;
                timeRemaining = 0;
                Timer.text = "00:00";
                StartBtn.interactable = true; // enable button after time ends
            }
        }
    }

    public void StartButton()
    {
        if (timerEnded)
        {
            QNAPanel.SetActive(true);
        }
    }
}

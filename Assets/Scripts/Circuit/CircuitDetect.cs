using UnityEngine;
using TMPro;
using System.Security.AccessControl;

public class CircuitDetect : MonoBehaviour
{
    [Header("Completion UI")]
    public GameObject CompUI;
    public GameObject HLevelMapUI;

    public GameObject TotalCarObj;
    public GameObject HLevelCarObject;

    

    int count = 0;
    int CircuitCount = 0;
    bool levelCompleted = false; // 🔥 new flag

    public GameObject Wall;
    public TextMeshProUGUI Text;

    [Header("Car Settings")]
    public CarController newcar; // drag CarController here in Inspector

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;

        HLevelCarObject.SetActive(false);
        Wall.SetActive(true);
        Text.text = "0";
    }

    void OnTriggerEnter(Collider other)
    {
        if (count == 0 && other.name == "Start")
        {
            count++;
            Debug.Log("Started round");
            Wall.SetActive(true);
        }
        else if (count == 1 && other.name == "1")
        {
            count++;
            Debug.Log("Checkpoint 1 reached");
        }
        else if (count == 2 && other.name == "2")
        {
            count++;
            Debug.Log("Checkpoint 2 reached");
        }
        else if (count == 3 && other.name == "End")
        {
            Debug.Log("Round completed!");
            count = 0;
            Wall.SetActive(false);
            CircuitCount++;

            Text.text = CircuitCount.ToString();
        }

        if (CircuitCount == 1 && !levelCompleted)
        {
            Debug.Log("You completed 3 rounds");
            CompUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;


            levelCompleted = true;   // 🔥 mark as completed

            if (newcar != null)
            {
                newcar.FullBreak();  // stop the car
            }
        }
    }

    public void NextButton()
    {
        CompUI.SetActive(false);
        HLevelMapUI.SetActive(true);
    }

    public void LevelStartButton()
    {
        TotalCarObj.SetActive(false);
        HLevelMapUI.SetActive(false);
        HLevelCarObject.SetActive(true);
    }

    public void ResetLevel()
    {
        if (levelCompleted) return; // 🚫 do nothing if level completed

        count = 0;
        CircuitCount = 0;
        Wall.SetActive(true);
        Text.text = "0";
        CompUI.SetActive(false);
    }
}

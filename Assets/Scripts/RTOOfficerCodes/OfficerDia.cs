using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // ✅ fixed

public class OfficerDia : MonoBehaviour
{
    [Header("UI References")]
    public Button nextButton;          // Assign your Button here
    public TMP_Text[] dialogueTexts;   // Assign all dialogue texts here (in order)

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;  // Adjustable typing speed

    private int currentIndex = 0;      // Tracks which dialogue is active
    private Coroutine typingCoroutine;
    public GameObject CancelButton;

    void Start()
    {
        CancelButton.SetActive(false);

        // Hide all texts at start
        foreach (var txt in dialogueTexts)
        {
            txt.gameObject.SetActive(false);
        }

        // Show first text instantly (no typing)
        dialogueTexts[0].gameObject.SetActive(true);

        // Button enabled
        nextButton.gameObject.SetActive(true);
    }

    // ✅ Assign this in Inspector → Button → OnClick
    public void OnNextButtonClicked()
    {
        // Stop current typing if running
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Hide current text
        dialogueTexts[currentIndex].gameObject.SetActive(false);

        // Go to next dialogue
        currentIndex++;

        // If still in range → show next
        if (currentIndex < dialogueTexts.Length)
        {
            TMP_Text nextText = dialogueTexts[currentIndex];
            nextText.gameObject.SetActive(true);

            // ✅ Apply typing animation only from 2nd text onward
            if (currentIndex > 0)
                typingCoroutine = StartCoroutine(TypeText(nextText));
        }

        // If last dialogue → disable button + show cancel
        if (currentIndex >= dialogueTexts.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            CancelButton.SetActive(true);

            // Load scene after delay

        }
    }

    private IEnumerator TypeText(TMP_Text textUI)
    {
        string fullText = textUI.text;
        textUI.text = ""; // Start empty

        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void scenechange()
    {
        Invoke("ChangeScene", 2f);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("First Level"); // ✅ fixed
    }

    
}

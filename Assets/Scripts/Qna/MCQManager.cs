using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Question
{
    [TextArea]
    public string questionText;
    public string[] options = new string[4];
    public int correctAnswerIndex; // 0 to 3
}

public class MCQManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text questionText;
    public TMP_Text[] optionTexts = new TMP_Text[4];
    public TMP_Text resultText;

    public Button nextButton;
    public Button restartButton;
    public GameObject congratsImage;

    [Header("Questions List")]
    public Question[] questions;

    private int currentQuestionIndex = 0;
    private int score = 0;

    [Header("UI obj")]
    public GameObject Level1;
    public GameObject Level2;

    void Start()
    {
        Level2.SetActive(false);
        nextButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        congratsImage.SetActive(false);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            EndQuiz();
            return;
        }

        Question q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        for (int i = 0; i < optionTexts.Length; i++)
            optionTexts[i].text = q.options[i];
    }

    public void OnOptionSelected(int index)
    {
        Question q = questions[currentQuestionIndex];

        if (index == q.correctAnswerIndex)
            score++;

        currentQuestionIndex++;
        ShowQuestion();
    }

    void EndQuiz()
    {
        float percentage = ((float)score / questions.Length) * 100f;

        questionText.text = "Quiz Completed!";
        foreach (var opt in optionTexts)
            opt.text = "";

        resultText.gameObject.SetActive(true);
        congratsImage.SetActive(true); // enable image for both cases

        if (percentage >= 70)
        {
            resultText.text = $"({score}/{questions.Length})";
            nextButton.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = $" {score}/{questions.Length}";
            nextButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
        }
    }

    public void OnRestart()
    {
        score = 0;
        currentQuestionIndex = 0;
        nextButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        congratsImage.SetActive(false);
        ShowQuestion();
    }

    public void OnNext()
    {
        Debug.Log("Next level or scene can load here...");
        Level1.SetActive(false);
         Level2.SetActive(true);
    }
}
